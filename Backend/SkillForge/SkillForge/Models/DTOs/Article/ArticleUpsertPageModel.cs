using SkillForge.Areas.Admin.Models.DTOs;

namespace SkillForge.Models.DTOs.Article;

public class ArticleUpsertPageModel
{
    public List<EntityOption> CategoryOptions { get; set; }

    public ArticleState? CurrentState { get; set; }
}
