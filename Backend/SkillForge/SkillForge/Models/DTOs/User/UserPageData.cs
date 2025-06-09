using SkillForge.Models.DTOs.Article;
using SkillForge.Models.DTOs.Tag;

namespace SkillForge.Models.DTOs.User;

public class UserPageData
{
    public string Name { get; set; }

    public string? Bio { get; set; }

    public string? AvatarImage { get; set; }

    public int FollowersCount { get; set; }

    public int FollowingsCount { get; set; }

    public int TagFollowingsCount { get; set; }

    public int ArticlesCount { get; set; }

    public bool IsFollowedByCurrentUser { get; set; }

    public List<TagListItem> LatestTagFollowings { get; set; }

    public List<UserListItem> LatestFollowers { get; set; }

    public List<UserListItem> LatestFollowings { get; set; }

    public List<TopArticleItem> TopArticles { get; set; }
}
