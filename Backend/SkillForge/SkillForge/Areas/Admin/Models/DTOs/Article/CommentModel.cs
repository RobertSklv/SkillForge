using SkillForge.Areas.Admin.Models.DTOs.Rating;
using SkillForge.Models.Database;

namespace SkillForge.Areas.Admin.Models.DTOs.Article;

public class CommentModel
{
    public UserLink User { get; set; }

    public string Content { get; set; }

    public RatingData RatingData { get; set; }

    public DateTime DateWritten { get; set; }
}
