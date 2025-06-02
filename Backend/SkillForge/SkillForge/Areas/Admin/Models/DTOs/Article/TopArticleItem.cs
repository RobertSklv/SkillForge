using SkillForge.Areas.Admin.Models.DTOs.Rating;

namespace SkillForge.Areas.Admin.Models.DTOs.Article;

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
