using SkillForge.Areas.Admin.Services;
using SkillForge.Attributes;
using SkillForge.Models.Database;

namespace SkillForge.Areas.Admin.Controllers;

[AdminNavLink("Users", "Index", SortOrder = 1)]
public class UserController : CrudController<User>
{
    protected override bool AddCreateActionOnIndexPage => false;

    public UserController(
        IUserService service)
        : base(service)
    {
    }
}