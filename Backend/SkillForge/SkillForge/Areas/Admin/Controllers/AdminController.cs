using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkillForge.Areas.Admin.Models.Components.Common;
using SkillForge.Areas.Admin.Models.Components.Pages;
using SkillForge.Extensions;

namespace SkillForge.Areas.Admin.Controllers;

[Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
[Area("Admin")]
public abstract class AdminController : Controller
{
    public int UserId => int.Parse(User.Claims.Where(c => c.Type == "Id").First().Value);

    public bool TryGetUserId([NotNullWhen(true)] out int? userId)
    {
        userId = null;

        try
        {
            userId = UserId;
        }
        catch (Exception)
        {
            return false;
        }

        return true;
    }

    protected void Alert(string message, ColorClass color)
    {
        Alert alert = new()
        {
            Content = message,
            Color = color
        };

        List<Alert> alerts = TempData.Get<List<Alert>>("Alerts") ?? new();
        alerts.Add(alert);
        TempData.Set("Alerts", alerts);
    }

    public PageActionButton BackAction(
        string? controller,
        string? area = "Admin",
        string? action = "Index",
        Dictionary<string, object>? requestParameters = null)
    {
        return new()
        {
            AreaName = area,
            ControllerName = controller,
            ActionName = action,
            IsLink = true,
            SortOrder = -1,
            Content = "Back",
            Color = ColorClass.Secondary,
            AlignToLeft = true,
            RequestParameters = requestParameters
        };
    }

    protected List<PageActionButton> GetOrCreatePageActionButtonsList()
    {
        if (ViewData["PageActions"] == null)
        {
            ViewData["PageActions"] = new List<PageActionButton>();
        }

        return (List<PageActionButton>)ViewData["PageActions"]!;
    }

    protected void AddBackAction(
        string action = "Index",
        string? controllerName = null,
        Dictionary<string, object>? requestParameters = null)
    {
        if (controllerName != null)
        {
            GetOrCreatePageActionButtonsList().Add(BackAction(
                area: "Admin",
                controller: controllerName,
                action: action,
                requestParameters: requestParameters));
        }
        else
        {
            GetOrCreatePageActionButtonsList().Add(BackAction(
                this,
                action: action,
                requestParameters: requestParameters));
        }
    }

    protected void AddCreateAction()
    {
        GetOrCreatePageActionButtonsList().Add(CreateAction(this));
    }

    private PageActionButton BackAction(
        Controller controller,
        string? action = "Index",
        Dictionary<string, object>? requestParameters = null)
    {
        string? area = controller.ControllerContext.RouteData.Values["area"] as string;
        string? controllerName = controller.ControllerContext.ActionDescriptor.ControllerName;

        return BackAction(controllerName, area, action, requestParameters);
    }

    private PageActionButton CreateAction(
        string? area,
        string? controller,
        string action = "Create",
        Dictionary<string, object>? requestParameters = null)
    {
        return new()
        {
            Content = "Create New",
            AreaName = area,
            ControllerName = controller,
            ActionName = action,
            Color = ColorClass.Primary,
            IsLink = true,
            RequestParameters = requestParameters
        };
    }

    private PageActionButton CreateAction(
        Controller controller,
        string action = "Create",
        Dictionary<string, object>? requestParameters = null)
    {
        string? area = controller.ControllerContext.RouteData.Values["area"] as string;
        string? controllerName = controller.ControllerContext.ActionDescriptor.ControllerName;

        return CreateAction(area, controllerName, action, requestParameters);
    }

    protected List<string> GetModelStateErrors()
    {
        List<string> errors = new();

        foreach (var value in ModelState.Values)
        {
            foreach (var error in value.Errors)
            {
                errors.Add(error.ErrorMessage);
            }
        }

        return errors;
    }

    protected SidebarLinkGroup GetOrCreateSidebarLinkGroup()
    {
        if (ViewData["SidebarLinks"] == null)
        {
            ViewData["SidebarLinks"] = new SidebarLinkGroup();
        }

        return (SidebarLinkGroup)ViewData["SidebarLinks"]!;
    }
}
