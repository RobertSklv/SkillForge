using SkillForge.Models.DTOs.Rating;
using SkillForge.Models.DTOs.Tag;
using SkillForge.Models.DTOs.User;

namespace SkillForge.Models.DTOs.Article;

public class ArticleCard
{
    public int ArticleId { get; set; }

    public UserLink Author { get; set; }

    public string Title { get; set; }

    public string? CoverImage { get; set; }

    public DateTime DatePublished { get; set; }

    public RatingData RatingData { get; set; }

    public List<TagLink> Tags { get; set; }

    public List<CommentModel> Comments { get; set; }

    public int TotalComments { get; set; }
}
