using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkillForge.Areas.Admin.Models.Components.Common;
using SkillForge.Areas.Admin.Models.Components.Pages;
using SkillForge.Areas.Admin.Models.DTOs;
using SkillForge.Areas.Admin.Models.User;
using SkillForge.Areas.Admin.Services;
using SkillForge.Attributes;
using SkillForge.Models.Database;

namespace SkillForge.Areas.Admin.Controllers;

[AdminNavMenu("users", "Users")]
[AdminNavLink("Users", "Index")]
public class UserController : CrudController<User>
{
    private readonly IUserService service;

    protected override bool AddCreateActionOnIndexPage => false;

    public UserController(IUserService service)
        : base(service)
    {
        this.service = service;
    }

    public override Task<IActionResult> Index([FromQuery] ListingModel listingModel)
    {
        CreateSuspendedAccountsLink();

        return base.Index(listingModel);
    }

    [HttpGet]
    public async Task<IActionResult> Suspended([FromQuery] ListingModel listingModel)
    {
        AddBackAction();

        return View(await service.CreateSuspendedAccountsListing(listingModel));
    }

    public override Task<IActionResult> View(int id)
    {
        return base.View(id);
    }

    public override async Task<IActionResult> ViewWithBackAction(User? user)
    {
        if (user != null)
        {
            user.Suspensions = await service.GetSuspensions(user.Id);

            if (!user.IsSuspended)
            {
                AddBackAction();

                CreateSuspendLink(user.Id);
            }
            else
            {
                AddBackAction("Suspended");
            }

            return base.View(user);
        }

        Alert($"User doesn't exist.", ColorClass.Danger);

        return RedirectToAction("Index");
    }

    [Authorize(Roles = "admin, mod")]
    public IActionResult Suspend(int id)
    {
        AddBackAction("View", requestParameters: new()
        {
            { "id", id }
        });

        SuspendUserFormData model = new()
        {
            UserId = id,
        };

        return View(model);
    }

    [Authorize(Roles = "admin, mod")]
    [HttpPost]
    public async Task<IActionResult> SuspendUser([FromForm] SuspendUserFormData data)
    {
        try
        {
            await service.Suspend(data.UserId, data.Violation, data.DurationDays, UserId);

            Alert("User account suspended", ColorClass.Success);
        }
        catch (Exception e)
        {
            Alert("An error ocurred: " + e.Message, ColorClass.Danger);
        }

        return RedirectToAction(nameof(Index));
    }

    public void CreateSuspendedAccountsLink()
    {
        GetOrCreatePageActionButtonsList().Add(new PageActionButton
        {
            AreaName = "Admin",
            ControllerName = "User",
            ActionName = "Suspended",
            Content = "Suspended Accounts",
            IsLink = true,
        });
    }

    public void CreateSuspendLink(int id)
    {
        GetOrCreatePageActionButtonsList().Add(new PageActionButton
        {
            Route = "/Admin/User/Suspend/" + id,
            Content = "Suspend",
            IsLink = true,
        });
    }
}