using Microsoft.AspNetCore.Mvc;
using SkillForge.Areas.Admin.Models.DTOs;
using SkillForge.Areas.Admin.Services;
using SkillForge.Attributes;
using SkillForge.Models.Database;

namespace SkillForge.Areas.Admin.Controllers;

[AdminNavLink("All Categories", "Index", Menu = "Categories", SortOrder = 1)]
public class CategoryController : CrudController<Category, CategoryVM>
{
    private readonly ICategoryService service;

    public CategoryController(
        ICategoryService service)
        : base(service)
    {
        this.service = service;
    }

    public override async Task<IActionResult> Create()
    {
        ViewData["ParentCategoryOptions"] = await service.GetPossibleParents();

        return await base.Create();
    }

    public override async Task<IActionResult> Edit(int id)
    {
        ViewData["ParentCategoryOptions"] = await service.GetPossibleParents(id);

        return await base.Edit(id);
    }
}