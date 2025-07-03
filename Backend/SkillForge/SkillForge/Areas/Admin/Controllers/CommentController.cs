using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkillForge.Areas.Admin.Models.Components.Common;
using SkillForge.Areas.Admin.Models.Components.Pages;
using SkillForge.Areas.Admin.Models.DTOs;
using SkillForge.Areas.Admin.Services;
using SkillForge.Attributes;
using SkillForge.Models.Database;

namespace SkillForge.Areas.Admin.Controllers;

[AdminNavMenu("comments", "Comments", SortOrder = 20)]
[AdminNavLink("All Comments", "Index")]
public class CommentController : CrudController<Comment>
{
    public const string VIEW_LINK = "view";
    public const string DELETE_LINK = "delete";
    public const string RESTORE_LINK = "restore";

    private readonly ICommentService service;

    protected override bool AddCreateActionOnIndexPage => false;

    public CommentController(ICommentService service)
        : base(service)
    {
        this.service = service;
    }

    public override Task<IActionResult> ViewWithBackAction(Comment? viewModel)
    {
        GenerateSidebarLinks(viewModel, VIEW_LINK);

        return base.ViewWithBackAction(viewModel);
    }

    [AdminNavLink("Deleted")]
    [HttpGet]
    [Authorize(Roles = "admin, mod")]
    public async Task<IActionResult> Deleted([FromQuery] ListingModel listingModel)
    {
        return View(await service.CreateDeletedCommentsListing(listingModel));
    }

    [HttpGet]
    public async new Task<IActionResult> Delete(int id)
    {
        Comment comment = await service.GetStrict(id);

        if (comment.DeleteReason != null)
        {
            return Redirect($"/Admin/Comment/View/{id}");
        }

        GenerateSidebarLinks(comment, DELETE_LINK);

        return View(comment);
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id, Violation deleteReason)
    {
        Comment comment = await service.GetStrict(id);

        try
        {
            await service.SoftDelete(comment, deleteReason);

            Alert("Comment successfully deleted", ColorClass.Success);
        }
        catch (Exception)
        {
            Alert("An error ocurred", ColorClass.Danger);
        }

        return Redirect($"/Admin/Comment/View/{id}");
    }

    [HttpGet]
    public async Task<IActionResult> Restore(int id)
    {
        Comment comment = await service.GetStrict(id);

        if (comment.DeleteReason == null)
        {
            return Redirect($"/Admin/Comment/View/{id}");
        }

        GenerateSidebarLinks(comment, RESTORE_LINK);

        return View(comment);
    }

    [HttpPost]
    public async Task<IActionResult> RestorePost(int id)
    {
        Comment comment = await service.GetStrict(id);

        try
        {
            await service.Restore(comment);

            Alert("Comment successfully restored", ColorClass.Success);
        }
        catch (Exception)
        {
            Alert("An error ocurred", ColorClass.Danger);
        }

        return Redirect($"/Admin/Comment/View/{id}");
    }

    public void GenerateSidebarLinks(Comment? comment, string activeLink)
    {
        if (comment == null)
        {
            return;
        }

        SidebarLinkGroup sidebar = GetOrCreateSidebarLinkGroup();
        sidebar.ActiveLinkId = activeLink;

        sidebar.Add(new SidebarLink
        {
            Id = VIEW_LINK,
            Content = "General",
            Route = $"/Admin/Comment/View/{comment.Id}",
        });

        if (comment.DeleteReason == null)
        {
            sidebar.Add(new SidebarLink
            {
                Id = DELETE_LINK,
                Content = "Delete",
                Route = $"/Admin/Comment/Delete/{comment.Id}",
            });
        }
        else
        {
            sidebar.Add(new SidebarLink
            {
                Id = DELETE_LINK,
                Content = "Restore",
                Route = $"/Admin/Comment/Restore/{comment.Id}",
            });
        }
    }
}