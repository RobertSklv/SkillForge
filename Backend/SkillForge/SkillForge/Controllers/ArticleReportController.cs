using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkillForge.Areas.Admin.Services;
using SkillForge.Models.DTOs.Report;

namespace SkillForge.Controllers;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class ArticleReportController : ApiController
{
    private readonly IArticleReportService service;

    public ArticleReportController(IArticleReportService service)
    {
        this.service = service;
    }

    [HttpPost]
    public async Task<IActionResult> Create(ReportCreateFormData form)
    {
        if (!TryGetUserId(out int? userId))
        {
            return Unauthorized();
        }

        await service.Create((int)userId, form);

        return Ok();
    }
}
