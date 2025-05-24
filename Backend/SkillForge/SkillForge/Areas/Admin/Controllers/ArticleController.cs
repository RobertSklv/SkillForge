using SkillForge.Areas.Admin.Services;
using SkillForge.Attributes;
using SkillForge.Models.Database;

namespace SkillForge.Areas.Admin.Controllers;

[AdminNavMenu("articles", "Articles")]
[AdminNavLink("All Articles", "Index")]
public class ArticleController : CrudController<Article>
{
    protected override bool AddCreateActionOnIndexPage => false;

    public ArticleController(
        IArticleService service)
        : base(service)
    {
    }
}