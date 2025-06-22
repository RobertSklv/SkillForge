using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkillForge.Areas.Admin.Models.Components.Common;
using SkillForge.Areas.Admin.Models.Components.Pages;
using SkillForge.Areas.Admin.Models.DTOs;
using SkillForge.Areas.Admin.Services;
using SkillForge.Attributes;
using SkillForge.Models.Database;

namespace SkillForge.Areas.Admin.Controllers;

[AdminNavLink("User Reports", "Index", Menu = "reports", SortOrder = 5)]
public class UserReportController : CrudController<UserReport>
{
    private readonly IUserReportService service;
    private readonly IUserService userService;

    protected override bool AddCreateActionOnIndexPage => false;

    public UserReportController(IUserReportService service, IUserService userService)
        : base(service)
    {
        this.service = service;
        this.userService = userService;
    }

    public override Task<IActionResult> Index([FromQuery] ListingModel listingModel)
    {
        CreateClosedUsersLink();

        return base.Index(listingModel);
    }

    [HttpGet]
    [Authorize(Roles = "admin, mod")]
    public async Task<IActionResult> Closed([FromQuery] ListingModel listingModel)
    {
        AddBackAction();

        return View(await service.CreateClosedReportsListing(listingModel));
    }

    public async Task<IActionResult> Close(int id)
    {
        bool success = await service.Close(id);

        if (success)
        {
            Alert("User report closed", ColorClass.Success);
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
                Alert("Users approved", ColorClass.Success);
            }
            else
            {
                Alert("An error ocurred", ColorClass.Danger);
            }
        }
        else throw new Exception("Action not defined");

        return nameof(Closed);
    }

    public void CreateClosedUsersLink()
    {
        GetOrCreatePageActionButtonsList().Add(new PageActionButton
        {
            AreaName = "Admin",
            ControllerName = "UserReport",
            ActionName = "Closed",
            Content = "Closed Reports",
            IsLink = true,
        });
    }
}