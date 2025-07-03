namespace SkillForge.Models.DTOs.Article;

public class ArticleState
{
    public ArticleUpsertFormData? Model { get; set; }

    public bool IsApproved { get; set; }
}
