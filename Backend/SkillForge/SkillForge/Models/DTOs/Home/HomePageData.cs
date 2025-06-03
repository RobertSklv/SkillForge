using SkillForge.Models.DTOs.Article;
using SkillForge.Models.DTOs.Tag;
using SkillForge.Models.DTOs.User;

namespace SkillForge.Models.DTOs.Home;

public class HomePageData
{
    public List<ArticleCard> LatestArticles { get; set; }

    public List<TopArticleItem> TopArticles { get; set; }

    public List<UserLink> TopUsers { get; set; }

    public List<TagLink> TopTags { get; set; }
}
