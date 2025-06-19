using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkillForge.Areas.Admin.Models.Components.Common;
using SkillForge.Areas.Admin.Models.DTOs;
using SkillForge.Areas.Admin.Services;
using SkillForge.Attributes;
using SkillForge.Models.Database;

namespace SkillForge.Areas.Admin.Controllers;

[AdminNavMenu("reports", "Reports", SortOrder = 100)]
[AdminNavLink("Article Reports", "Index", Menu = "reports", SortOrder = 1)]
public class ArticleReportController : CrudController<ArticleReport>
{
    private readonly IArticleReportService service;
    private readonly IArticleService articleService;

    public ArticleReportController(IArticleReportService service, IArticleService articleService)
        : base(service)
    {
        this.service = service;
        this.articleService = articleService;
    }

    [AdminNavLink("Closed")]
    [HttpGet]
    [Authorize(Roles = "admin, mod")]
    public async Task<IActionResult> Closed([FromQuery] ListingModel listingModel)
    {
        return View(await service.CreateClosedReportsListing(listingModel));
    }

    [Authorize(Roles = "admin, mod")]
    [Route("/Admin/ArticleReport/DeleteArticle/{id}")]
    public async Task<IActionResult> DeleteArticle([FromRoute] int id, [FromForm] int articleReportId)
    {
        try
        {
            await articleService.SoftDelete(id, articleReportId);

            Alert("Article deleted", ColorClass.Success);
        }
        catch (Exception e)
        {
            Alert("An error ocurred: " + e.Message, ColorClass.Danger);
        }

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Close(int id)
    {
        bool success = await service.Close(id);

        if (success)
        {
            Alert("Article report closed", ColorClass.Success);
        }
        else
        {
            Alert("An error ocurred", ColorClass.Danger);
        }

        return RedirectToAction(nameof(Closed));
    }

    protected override async Task<string> MassAction(string massAction, List<int> selectedItemIds)
    {
        if (massAction == "MassClose")
        {
            if (!TryGetUserId(out int? adminId))
            {
                throw new Exception("Admin User Id not found!");
            }

            bool success = await service.MassClose(selectedItemIds);

            if (success)
            {
                Alert("Articles approved", ColorClass.Success);
            }
            else
            {
                Alert("An error ocurred", ColorClass.Danger);
            }
        }
        else throw new Exception("Action not defined");

        return nameof(Closed);
    }
}