using SkillForge.Areas.Admin.Models.DTOs;
using SkillForge.Areas.Admin.Services;
using SkillForge.Attributes;
using SkillForge.Models.Database;

namespace SkillForge.Areas.Admin.Controllers;

[AdminNavLink("All Categories", "Index", Menu = "Categories", SortOrder = 1)]
public class CategoryController : CrudController<Category, CategoryVM>
{
    public CategoryController(
        ICategoryService service)
        : base(service)
    {
    }
}