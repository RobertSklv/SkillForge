namespace SkillForge.Areas.Admin.Models.DTOs.Article;

public class ArticleUpsertPageModel
{
    public List<EntityOption> CategoryOptions { get; set; }

    public ArticleState? CurrentState { get; set; }
}
