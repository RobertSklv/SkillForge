using SkillForge.Models.DTOs.Rating;
using SkillForge.Models.DTOs.User;

namespace SkillForge.Models.DTOs.Article;

public class ArticlePageModel
{
    public int ArticleId { get; set; }

    public UserLink Author { get; set; }

    public string? CoverImage { get; set; }

    public string Title { get; set; }

    public string Content { get; set; }

    public string CategoryName { get; set; }

    public string CategoryCode { get; set; }

    public DateTime DatePublished { get; set; }

    public List<string> Tags { get; set; }

    public List<CommentModel> Comments { get; set; }

    public RatingData RatingData { get; set; }

    public int Views { get; set; }
}
