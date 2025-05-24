using Microsoft.AspNetCore.Mvc;
using SkillForge.Areas.Admin.Models.Components.Navigation;
using SkillForge.Areas.Admin.Services;

namespace SkillForge.Areas.Admin.Models.ViewComponents;

public class AdminNavigationViewComponent : ViewComponent
{
    private readonly IAdminNavigationService adminNavigationService;

    public AdminNavigationViewComponent(IAdminNavigationService adminNavigationService)
    {
        this.adminNavigationService = adminNavigationService;
    }

    public IViewComponentResult Invoke()
    {
        Nav? nav = adminNavigationService.CreateNav();

        return View(nav);
    }
}