namespace SkillForge.Areas.Admin.Models.DTOs.Article;

public class ArticleState
{
    public ArticleUpsertDTO? Model { get; set; }

    public bool IsApproved { get; set; }
}
