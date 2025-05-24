using SkillForge.Areas.Admin.Models;
using SkillForge.Areas.Admin.Models.Components.Navigation;

namespace SkillForge.Areas.Admin.Services;

public interface IAdminNavigationService
{
    Nav? CreateNav();

    string? GetActionRoute(NavLinkDefinition def);

    string AddTrailingSlash(string route);

    string? GetCurrentRoute();
}