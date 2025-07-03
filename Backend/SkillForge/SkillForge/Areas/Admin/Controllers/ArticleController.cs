using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkillForge.Areas.Admin.Models.Components.Common;
using SkillForge.Areas.Admin.Models.Components.Pages;
using SkillForge.Areas.Admin.Models.DTOs;
using SkillForge.Areas.Admin.Services;
using SkillForge.Attributes;
using SkillForge.Models.Database;

namespace SkillForge.Areas.Admin.Controllers;

[AdminNavMenu("articles", "Articles", SortOrder = 10)]
[AdminNavLink("All Articles", "Index")]
public class ArticleController : CrudController<Article>
{
    public const string VIEW_LINK = "view";
    public const string DELETE_LINK = "delete";
    public const string RESTORE_LINK = "restore";

    private readonly IArticleService service;

    protected override bool AddCreateActionOnIndexPage => false;

    public ArticleController(
        IArticleService service)
        : base(service)
    {
        ListingTitle = "All articles";
        this.service = service;
    }

    public override Task<IActionResult> ViewWithBackAction(Article? viewModel)
    {
        GenerateSidebarLinks(viewModel, VIEW_LINK);

        return base.ViewWithBackAction(viewModel);
    }

    [AdminNavLink("Pending")]
    [HttpGet]
    [Authorize(Roles = "admin, mod")]
    public async Task<IActionResult> Pending([FromQuery] ListingModel listingModel)
    {
        return View(await service.CreatePendingArticlesListing(listingModel));
    }

    [AdminNavLink("Deleted")]
    [HttpGet]
    [Authorize(Roles = "admin, mod")]
    public async Task<IActionResult> Deleted([FromQuery] ListingModel listingModel)
    {
        return View(await service.CreateDeletedArticlesListing(listingModel));
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

    [HttpGet]
    public async new Task<IActionResult> Delete(int id)
    {
        Article article = await service.GetStrict(id);

        if (article.DeleteReason != null)
        {
            return Redirect($"/Admin/Article/View/{id}");
        }

        AddBackAction();
        GenerateSidebarLinks(article, DELETE_LINK);

        return View(article);
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id, Violation deleteReason)
    {
        Article article = await service.GetStrict(id);

        try
        {
            await service.SoftDelete(article, deleteReason);

            Alert("Article successfully deleted", ColorClass.Success);
        }
        catch (Exception)
        {
            Alert("An error ocurred", ColorClass.Danger);
        }

        return Redirect($"/Admin/Article/View/{id}");
    }

    [HttpGet]
    public async Task<IActionResult> Restore(int id)
    {
        Article article = await service.GetStrict(id);

        if (article.DeleteReason == null)
        {
            return Redirect($"/Admin/Article/View/{id}");
        }

        GenerateSidebarLinks(article, RESTORE_LINK);

        return View(article);
    }

    [HttpPost]
    public async Task<IActionResult> RestorePost(int id)
    {
        Article article = await service.GetStrict(id);

        try
        {
            await service.Restore(article);

            Alert("Article successfully restored", ColorClass.Success);
        }
        catch (Exception)
        {
            Alert("An error ocurred", ColorClass.Danger);
        }

        return Redirect($"/Admin/Article/View/{id}");
    }

    public void GenerateSidebarLinks(Article? article, string activeLink)
    {
        if (article == null)
        {
            return;
        }

        SidebarLinkGroup sidebar = GetOrCreateSidebarLinkGroup();
        sidebar.ActiveLinkId = activeLink;

        sidebar.Add(new SidebarLink
        {
            Id = VIEW_LINK,
            Content = "General",
            Route = $"/Admin/Article/View/{article.Id}",
        });

        if (article.DeleteReason == null)
        {
            sidebar.Add(new SidebarLink
            {
                Id = DELETE_LINK,
                Content = "Delete",
                Route = $"/Admin/Article/Delete/{article.Id}",
            });
        }
        else
        {
            sidebar.Add(new SidebarLink
            {
                Id = DELETE_LINK,
                Content = "Restore",
                Route = $"/Admin/Article/Restore/{article.Id}",
            });
        }
    }
}