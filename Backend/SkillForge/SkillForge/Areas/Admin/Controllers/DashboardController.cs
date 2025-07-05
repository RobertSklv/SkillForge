using Microsoft.AspNetCore.Mvc;
using SkillForge.Attributes;

namespace SkillForge.Areas.Admin.Controllers;

[AdminNavLink("Hangfire Dashboard", Route = "/Admin/Hangfire", SortOrder = 150)]
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
