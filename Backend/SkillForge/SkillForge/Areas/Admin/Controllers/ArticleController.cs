using Microsoft.AspNetCore.Mvc;
using SkillForge.Areas.Admin.Models.DTOs;
using SkillForge.Areas.Admin.Services;
using SkillForge.Attributes;
using SkillForge.Models.Database;

namespace SkillForge.Areas.Admin.Controllers;

[AdminNavMenu("articles", "Articles")]
[AdminNavLink("All Articles", "Index")]
public class ArticleController : CrudController<Article>
{
    private readonly IArticleService service;

    protected override bool AddCreateActionOnIndexPage => false;

    public ArticleController(
        IArticleService service)
        : base(service)
    {
        ListingTitle = "All articles";
        this.service = service;
    }

    [AdminNavLink("Pending")]
    [HttpGet]
    public virtual async Task<IActionResult> Pending([FromQuery] ListingModel listingModel)
    {
        return View(await service.CreatePendingArticlesListing(listingModel));
    }
}