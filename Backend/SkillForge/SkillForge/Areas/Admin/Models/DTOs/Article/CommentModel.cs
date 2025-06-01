using SkillForge.Areas.Admin.Models.DTOs.Rating;

namespace SkillForge.Areas.Admin.Models.DTOs.Article;

public class CommentModel
{
    public int CommentId { get; set; }

    public UserLink User { get; set; }

    public string Content { get; set; }

    public RatingData RatingData { get; set; }

    public DateTime DateWritten { get; set; }
}
