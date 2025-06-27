using SkillForge.Models.DTOs.Rating;
using SkillForge.Models.DTOs.User;

namespace SkillForge.Models.DTOs.Article;

public class CommentModel
{
    public int CommentId { get; set; }

    public UserLink User { get; set; }

    public string Content { get; set; }

    public RatingData RatingData { get; set; }

    public DateTime DateWritten { get; set; }

    public DateTime? DateEdited { get; set; }
}
