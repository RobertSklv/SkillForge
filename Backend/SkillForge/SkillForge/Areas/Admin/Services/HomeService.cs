using SkillForge.Models.DTOs.Article;
using SkillForge.Models.DTOs.Home;
using SkillForge.Models.DTOs.Tag;
using SkillForge.Models.DTOs.User;

namespace SkillForge.Areas.Admin.Services;

public class HomeService : IHomeService
{
    private readonly IArticleService articleService;
    private readonly IUserService userService;
    private readonly ITagService tagService;

    public HomeService(
        IArticleService articleService,
        IUserService userService,
        ITagService tagService)
    {
        this.articleService = articleService;
        this.userService = userService;
        this.tagService = tagService;
    }

    public async Task<HomePageData> LoadPage()
    {
        List<ArticleCard> latestArticles = await articleService.GetLatest(0, 10);
        List<TopArticleItem> topArticles = await articleService.GetMostPopularItems();
        List<UserLink> topUsers = await userService.GetMostPopularLinks();
        List<TagLink> topTags = await tagService.GetMostPopularLinks();

        return new HomePageData
        {
            LatestArticles = latestArticles,
            TopArticles = topArticles,
            TopUsers = topUsers,
            TopTags = topTags,
        };
    }
}
