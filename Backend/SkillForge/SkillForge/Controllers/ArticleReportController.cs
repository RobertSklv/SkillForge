using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkillForge.Areas.Admin.Services;
using SkillForge.Models.DTOs.ArticleReport;

namespace SkillForge.Controllers;

[Authorize(AuthenticationSchemes = "FrontendCookie")]
public class ArticleReportController : ApiController
{
    private readonly IArticleReportService service;

    public ArticleReportController(IArticleReportService service)
    {
        this.service = service;
    }

    [HttpGet]
    public IActionResult FormOptions()
    {
        return Ok(service.GetFormOptions());
    }

    [HttpPost]
    public async Task<IActionResult> Create(ArticleReportCreate form)
    {
        if (!TryGetUserId(out int? userId))
        {
            return Unauthorized();
        }

        await service.Create((int)userId, form);

        return Ok();
    }
}
