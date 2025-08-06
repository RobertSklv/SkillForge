using SkillForge.Areas.Admin.Services;
using SkillForge.Attributes;
using SkillForge.Models.Database;

namespace SkillForge.Areas.Admin.Controllers;

[AdminNavMenu("analytics", "Analytics", SortOrder = 90)]
[AdminNavLink("Guest Article Views", "Index", SortOrder = 1)]
public class GuestArticleViewController : CrudController<GuestArticleView>
{
    protected override bool AddCreateActionOnIndexPage => false;

    public GuestArticleViewController(IGuestArticleViewService service)
        : base(service)
    {
    }
}