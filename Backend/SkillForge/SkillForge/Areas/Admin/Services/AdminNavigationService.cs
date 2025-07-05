using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using SkillForge.Areas.Admin.Controllers;
using SkillForge.Areas.Admin.Models;
using SkillForge.Areas.Admin.Models.Components.Navigation;
using SkillForge.Attributes;
using System.Reflection;

namespace SkillForge.Areas.Admin.Services;

public class AdminNavigationService : IAdminNavigationService
{
    public static Assembly? MainAssembly;

    private readonly IUrlHelperFactory urlHelperFactory;
    private readonly IActionContextAccessor actionContextAccessor;

    public AdminNavigationService(IUrlHelperFactory urlHelperFactory, IActionContextAccessor actionContextAccessor)
    {
        this.urlHelperFactory = urlHelperFactory;
        this.actionContextAccessor = actionContextAccessor;
    }

    public Nav? CreateNav()
    {
        Assembly? assembly = MainAssembly ?? Assembly.GetEntryAssembly();

        if (assembly == null)
        {
            return null;
        }

        Type[] types = assembly.GetTypes();

        List<AdminNavMenuAttribute> menuAttributes = new();
        List<AdminNavLinkAttribute> linkAttributes = new();
        List<NavLinkDefinition> navLinkDefs = new();

        string? currentRoute = GetCurrentRoute();

        foreach (Type type in types)
        {
            if (type.IsSubclassOf(typeof(AdminController)))
            {
                IEnumerable<AdminNavMenuAttribute> navMenus = type.GetCustomAttributes<AdminNavMenuAttribute>();
                menuAttributes.AddRange(navMenus);

                IEnumerable<AdminNavLinkAttribute> controllerDefinedNavLinks = type.GetCustomAttributes<AdminNavLinkAttribute>();
                linkAttributes.AddRange(controllerDefinedNavLinks);

                MethodInfo[] methods = type.GetMethods();

                AdminNavMenuAttribute? firstMenu = navMenus.FirstOrDefault();
                foreach (AdminNavLinkAttribute link in controllerDefinedNavLinks)
                {
                    if (firstMenu != null && link.Menu == null)
                    {
                        link.Menu = firstMenu.code;
                    }

                    NavLinkDefinition def = new()
                    {
                        ControllerType = type,
                        Attribute = link,
                    };

                    if (link.ActionName != null)
                    {
                        def.Method = type.GetMethod(link.ActionName)
                            ?? throw new Exception($"Action {link.ActionName} was not found inside the {type.Name} controller.");
                    }
                    else if (link.Route != null)
                    {
                        def.Route = link.Route;
                    }
                    else throw new Exception($"Either {nameof(AdminNavLinkAttribute.ActionName)} or {nameof(AdminNavLinkAttribute.Route)} must be defined for a nav link.");

                    navLinkDefs.Add(def);
                }

                foreach (MethodInfo methodInfo in methods)
                {
                    AdminNavLinkAttribute? navLinkAttr = methodInfo.GetCustomAttribute<AdminNavLinkAttribute>();

                    if (navLinkAttr == null)
                    {
                        continue;
                    }

                    if (firstMenu != null && navLinkAttr.Menu == null)
                    {
                        navLinkAttr.Menu = firstMenu.code;
                    }

                    navLinkDefs.Add(new()
                    {
                        ControllerType = type,
                        Method = methodInfo,
                        Attribute = navLinkAttr,
                    });
                }
            }
        }

        Dictionary<string, NavMenu> menus = new();
        List<NavLink> independentLinks = new();

        foreach (AdminNavMenuAttribute menuAttr in menuAttributes)
        {
            NavMenu menu = new()
            {
                Icon = new()
                {
                    IconClass = menuAttr.IconClass
                },
                Name = menuAttr.displayedName,
                Code = menuAttr.code,
                SortOrder = menuAttr.SortOrder
            };
            menus.Add(menu.Code, menu);
        }

        NavLinkDefinition? activeNavLink = navLinkDefs
            .Where(def =>
            {
                string? route = GetActionRoute(def);

                return route == currentRoute;
            })
            .FirstOrDefault();

        foreach (NavLinkDefinition def in navLinkDefs)
        {
            string? route = GetActionRoute(def);
            AdminNavLinkAttribute navLinkAttr = def.Attribute;

            bool isActive;

            if (activeNavLink != null)
            {
                isActive = def == activeNavLink;
            }
            else
            {
                isActive = currentRoute != null &&
                    route != null &&
                    currentRoute.StartsWith(AddTrailingSlash(route));
            }

            NavLink link = new()
            {
                Name = navLinkAttr.displayedName,
                SortOrder = navLinkAttr.SortOrder,
                Icon = new()
                {
                    IconClass = navLinkAttr.IconClass
                },
                Url = route,
                IsActive = isActive
            };

            if (navLinkAttr.Menu == null)
            {
                independentLinks.Add(link);

                continue;
            }

            if (!menus.ContainsKey(navLinkAttr.Menu))
            {
                menus.Add(navLinkAttr.Menu, new()
                {
                    Name = navLinkAttr.Menu,
                });
            }

            menus[navLinkAttr.Menu].Links.Add(link);
        }

        Nav nav = new();

        nav.Items.AddRange(menus.Values);
        nav.Items.AddRange(independentLinks);

        nav.Items.Sort((a, b) => a.SortOrder - b.SortOrder);

        foreach (NavItem item in nav.Items)
        {
            if (item is NavMenu menu)
            {
                menu.Links.Sort((a, b) => a.SortOrder - b.SortOrder);
            }
        }

        foreach (NavMenu menu in menus.Values)
        {
            foreach (NavLink menuLink in menu.Links)
            {
                if (menuLink.IsActive)
                {
                    menu.IsActive = true;
                    break;
                }
            }
        }

        return nav;
    }

    public string? GetActionRoute(NavLinkDefinition def)
    {
        if (def.Method == null)
        {
            return def.Route;
        }

        RouteAttribute? routeAttr = def.Method.GetCustomAttribute<RouteAttribute>();

        if (routeAttr != null)
        {
            return routeAttr.Template;
        }

        if (def.ControllerType == null)
        {
            throw new Exception("Controller type is null");
        }

        string areaName = def.ControllerType.GetCustomAttribute<AreaAttribute>()?.RouteValue ?? "Admin";
        string controllerName = def.ControllerType.Name[..^"Controller".Length];
        string actionName = def.Method.Name;

        IUrlHelper urlHelper = urlHelperFactory.GetUrlHelper(actionContextAccessor.ActionContext);

        return urlHelper.Action(actionName, controllerName, new
        {
            Area = areaName
        });
    }

    public string AddTrailingSlash(string route)
    {
        if (!route.EndsWith("/"))
        {
            route += "/";
        }

        return route;
    }

    public string? GetCurrentRoute()
    {
        IUrlHelper urlHelper = urlHelperFactory.GetUrlHelper(actionContextAccessor.ActionContext);

        return urlHelper.Action(
            actionContextAccessor.ActionContext.RouteData.Values["action"] as string,
            actionContextAccessor.ActionContext.RouteData.Values["controller"] as string,
            new
            {
                Area = actionContextAccessor.ActionContext.RouteData.Values["area"] as string
            });
    }
}
