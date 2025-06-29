using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkillForge.Areas.Admin.Models.DTOs;
using SkillForge.Areas.Admin.Services;
using SkillForge.Attributes;
using SkillForge.Exceptions;
using SkillForge.Models.Database;

namespace SkillForge.Areas.Admin.Controllers;

[AdminNavLink("Administrators", "Index", Menu = "users", SortOrder = 1)]
public class AdminUserController : CrudController<AdminUser, AdminUserVM>
{
    private readonly IAdminUserService service;
    private readonly IAdminRoleService adminRoleService;

    protected override bool AddCreateActionOnIndexPage => User.Identity != null && User.IsInRole("admin");

    public AdminUserController(IAdminUserService service, IAdminRoleService adminRoleService)
        : base(service)
    {
        this.service = service;
        this.adminRoleService = adminRoleService;
    }

    [Authorize(Roles = "admin")]
    public override async Task<IActionResult> Create()
    {
        ViewData["Roles"] = await adminRoleService.GetAll();

        return await base.Create();
    }

    [Authorize(Roles = "admin")]
    public override async Task<IActionResult> Edit(int id)
    {
        ViewData["Roles"] = await adminRoleService.GetAll();

        return await base.Edit(id);
    }

    [HttpGet]
    public async Task<IActionResult> EditCurrent()
    {
        AdminUser user = await service.GetStrict(UserId);
        AdminUserVM viewModel = service.EntityToViewModel(user);

        ViewData["Roles"] = await adminRoleService.GetAll();

        return await Edit(viewModel);
    }

    public override Task UpsertMethod(AdminUserVM model)
    {
        return service.Upsert(model, User.IsInRole("admin"));
    }

    public override IActionResult RedirectBackToEditPage(int id)
    {
        if (!User.IsInRole("admin"))
        {
            return RedirectToAction(nameof(EditCurrent));
        }

        return base.RedirectBackToEditPage(id);
    }

    public override async Task<IActionResult> Create(AdminUserVM model)
    {
        try
        {
            return await base.Create(model);
        }
        catch (ModelValidationException e)
        {
            ModelState.AddModelError(e.FieldName, e.Message);

            return ValidationProblem(ModelState);
        }
    }

    public override async Task<IActionResult> Update(AdminUserVM model)
    {
        try
        {
            return await base.Update(model);
        }
        catch (ModelValidationException e)
        {
            ModelState.AddModelError(e.FieldName, e.Message);

            return ValidationProblem(ModelState);
        }
    }
}