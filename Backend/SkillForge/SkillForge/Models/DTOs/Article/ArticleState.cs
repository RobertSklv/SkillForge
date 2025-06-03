namespace SkillForge.Models.DTOs.Article;

public class ArticleState
{
    public ArticleUpsertDTO? Model { get; set; }

    public bool IsApproved { get; set; }
}
