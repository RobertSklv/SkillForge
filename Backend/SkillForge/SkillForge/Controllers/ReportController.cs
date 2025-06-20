using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkillForge.Areas.Admin.Services;

namespace SkillForge.Controllers;

[Authorize(AuthenticationSchemes = "FrontendCookie")]
public class ReportController : ApiController
{
    private readonly IReportService service;

    public ReportController(IReportService service)
    {
        this.service = service;
    }

    [HttpGet]
    public IActionResult FormOptions()
    {
        return Ok(service.GetFormOptions());
    }
}
