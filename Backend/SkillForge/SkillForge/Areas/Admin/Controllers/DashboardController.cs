using Microsoft.AspNetCore.Mvc;

namespace SkillForge.Areas.Admin.Controllers;

public class DashboardController : AdminController
{
    [Route("/")]
    [Route("/Admin")]
    [Route("/Admin/Dashboard")]
    public IActionResult Index()
    {
        return View();
    }
}
