using SkillForge.Areas.Admin.Models.DTOs.Article;

namespace SkillForge.Areas.Admin.Models.DTOs;

public class HomePageData
{
    public List<ArticleCard> LatestArticles { get; set; }

    public List<TopArticleItem> TopArticles { get; set; }
    
    public List<UserLink> TopUsers { get; set; }
    
    public List<TagLink> TopTags { get; set; }
}
