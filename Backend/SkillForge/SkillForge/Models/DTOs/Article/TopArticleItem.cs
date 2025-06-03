using SkillForge.Models.DTOs.Rating;
using SkillForge.Models.DTOs.User;

namespace SkillForge.Models.DTOs.Article;

public class TopArticleItem
{
    public int ArticleId { get; set; }

    public UserLink Author { get; set; }

    public string Title { get; set; }

    public int ViewCount { get; set; }

    public int CommentCount { get; set; }

    public DateTime DatePublished { get; set; }

    public RatingData RatingData { get; set; }
}
