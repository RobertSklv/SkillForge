using SkillForge.Models.DTOs.Article;
using SkillForge.Models.DTOs.Home;
using SkillForge.Models.DTOs.Tag;
using SkillForge.Models.DTOs.User;
using SkillForge.Services;

namespace SkillForge.Areas.Admin.Services;

public class HomeService : IHomeService
{
    private readonly IUserService userService;
    private readonly ITagService tagService;
    private readonly IUserFeedService userFeedService;

    public HomeService(
        IUserService userService,
        ITagService tagService,
        IUserFeedService userFeedService)
    {
        this.userService = userService;
        this.tagService = tagService;
        this.userFeedService = userFeedService;
    }

    public async Task<HomePageData> LoadPage(int? userId)
    {
        List<TopArticleItem> topArticles = await userFeedService.GetTopArticles(5);
        List<UserLink> topUsers = await userService.GetMostPopularLinks();
        List<TagLink> topTags = await tagService.GetMostFollowedLinks();
        List<ArticleCard> latestArticles = await userFeedService.GetLatestArticles(userId, 0, 6);

        return new HomePageData
        {
            TopArticles = topArticles,
            TopUsers = topUsers,
            TopTags = topTags,
            LatestArticles = latestArticles,
        };
    }
}
