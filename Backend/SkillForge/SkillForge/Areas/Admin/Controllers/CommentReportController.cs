using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkillForge.Areas.Admin.Models.Components.Common;
using SkillForge.Areas.Admin.Models.Components.Pages;
using SkillForge.Areas.Admin.Models.DTOs;
using SkillForge.Areas.Admin.Services;
using SkillForge.Attributes;
using SkillForge.Models.Database;

namespace SkillForge.Areas.Admin.Controllers;

[AdminNavLink("Comment Reports", "Index", Menu = "reports", SortOrder = 3)]
public class CommentReportController : CrudController<CommentReport>
{
    private readonly ICommentReportService service;
    private readonly ICommentService commentService;

    protected override bool AddCreateActionOnIndexPage => false;

    public CommentReportController(ICommentReportService service, ICommentService commentService)
        : base(service)
    {
        this.service = service;
        this.commentService = commentService;
    }

    public override Task<IActionResult> Index([FromQuery] ListingModel listingModel)
    {
        CreateClosedCommentReportsLink();

        return base.Index(listingModel);
    }

    [HttpGet]
    [Authorize(Roles = "admin, mod")]
    public async Task<IActionResult> Closed([FromQuery] ListingModel listingModel)
    {
        AddBackAction();

        return View(await service.CreateClosedReportsListing(listingModel));
    }

    [Authorize(Roles = "admin, mod")]
    [Route("/Admin/CommentReport/DeleteComment/{id}")]
    public async Task<IActionResult> DeleteComment([FromRoute] int id, [FromForm] int commentReportId)
    {
        try
        {
            await commentService.SoftDelete(id, commentReportId);

            Alert("Comment deleted", ColorClass.Success);
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
            Alert("Comment report closed", ColorClass.Success);
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
                Alert("Comments approved", ColorClass.Success);
            }
            else
            {
                Alert("An error ocurred", ColorClass.Danger);
            }
        }
        else throw new Exception("Action not defined");

        return nameof(Closed);
    }

    public void CreateClosedCommentReportsLink()
    {
        GetOrCreatePageActionButtonsList().Add(new PageActionButton
        {
            AreaName = "Admin",
            ControllerName = "CommentReport",
            ActionName = "Closed",
            Content = "Closed Reports",
            IsLink = true,
        });
    }
}