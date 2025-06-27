namespace SkillForge.Models.DTOs.Comment;

public class CommentUpsertFormData
{
    public int? ArticleId { get; set; }

    public int CommentId { get; set; }

    public string Content { get; set; }
}
