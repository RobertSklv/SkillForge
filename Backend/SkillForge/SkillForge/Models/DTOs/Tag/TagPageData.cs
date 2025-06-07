using SkillForge.Models.DTOs.Article;
using SkillForge.Models.DTOs.User;

namespace SkillForge.Models.DTOs.Tag;

public class TagPageData
{
    public string Name { get; set; }

    public string? Description { get; set; }

    public int FollowersCount { get; set; }

    public int ArticlesCount { get; set; }

    public bool IsFollowedByCurrentUser { get; set; }

    public List<UserListItem> LatestFollowers { get; set; }

    public List<TopArticleItem> TopArticles { get; set; }

    public List<ArticleCard> LatestArticles { get; set; }
}
