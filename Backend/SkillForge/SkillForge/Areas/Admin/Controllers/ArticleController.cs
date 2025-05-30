using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkillForge.Areas.Admin.Models.Components.Common;
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
    [Authorize(Roles = "admin, mod")]
    public async Task<IActionResult> Pending([FromQuery] ListingModel listingModel)
    {
        return View(await service.CreatePendingArticlesListing(listingModel));
    }

    public async Task<IActionResult> Preview(int id)
    {
        Article article = await service.GetStrict(id);

        if (article.ApprovalId != null)
        {
            return BadRequest("Article already approved.");
        }

        AddBackAction("Pending");

        return View(article);
    }

    [Authorize(Roles = "admin, mod")]
    public async Task<IActionResult> Approve(int id)
    {
        if (!TryGetUserId(out int? adminId))
        {
            throw new Exception("Admin User Id not found!");
        }

        bool success = await service.Approve(id, (int)adminId);

        if (success)
        {
            Alert("Article approved", ColorClass.Success);
        }
        else
        {
            Alert("An error ocurred", ColorClass.Danger);
        }

        return RedirectToAction(nameof(Pending));
    }

    protected override async Task<string> MassAction(string massAction, List<int> selectedItemIds)
    {
        if (massAction == "MassApprove")
        {
            if (!TryGetUserId(out int? adminId))
            {
                throw new Exception("Admin User Id not found!");
            }

            bool success = await service.MassApprove(selectedItemIds, (int)adminId);

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

        return nameof(Pending);
    }
}