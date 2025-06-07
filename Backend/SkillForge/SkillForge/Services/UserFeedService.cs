using Azure;
using SkillForge.Areas.Admin.Repositories;
using SkillForge.Areas.Admin.Services;
using SkillForge.Models.Database;
using SkillForge.Models.DTOs.Article;
using SkillForge.Models.DTOs.User;

namespace SkillForge.Services;

public class UserFeedService : IUserFeedService
{
    private readonly IArticleRepository articleRepository;
    private readonly ITagRepository tagRepository;
    private readonly ICommentRepository commentRepository;
    private readonly IUserRepository userRepository;
    private readonly IFrontendService frontendService;

    public UserFeedService(
		IArticleRepository articleRepository,
        ITagRepository tagRepository,
        ICommentRepository commentRepository,
        IUserRepository userRepository,
        IFrontendService frontendService)
	{
        this.articleRepository = articleRepository;
        this.tagRepository = tagRepository;
        this.commentRepository = commentRepository;
        this.userRepository = userRepository;
        this.frontendService = frontendService;
    }

    public async Task<List<ArticleCard>> GetLatestArticles(int? userId, int batchIndex, int batchSize)
    {
        List<ArticleCard> latest = (await articleRepository.GetLatest(batchIndex, batchSize)).ConvertAll(frontendService.CreateArticleCard);

        if (userId != null)
        {
            await SetCurrentUserRating(latest, (int)userId);
        }

        return latest;
    }

    public async Task<List<ArticleCard>> GetLatestArticlesByTag(int tagId, int? userId, int batchIndex, int batchSize)
    {
        List<ArticleCard> latest = (await articleRepository.GetLatestByTag(tagId, batchIndex, batchSize)).ConvertAll(frontendService.CreateArticleCard);

        if (userId != null)
        {
            await SetCurrentUserRating(latest, (int)userId);
        }

        return latest;
    }

    public async Task<List<ArticleCard>> GetLatestArticlesByTag(string tagName, int? userId, int batchIndex, int batchSize)
    {
        List<ArticleCard> latest = (await articleRepository.GetLatestByTag(tagName, batchIndex, batchSize)).ConvertAll(frontendService.CreateArticleCard);

        if (userId != null)
        {
            await SetCurrentUserRating(latest, (int)userId);
        }

        return latest;
    }

    public async Task SetCurrentUserRating(List<ArticleCard> articleCards, int userId)
    {
        List<int> articleIds = articleCards.ConvertAll(a => a.ArticleId);
        List<ArticleRating> userRatings = await articleRepository.GetUserRating(userId, articleIds);

        foreach (ArticleCard a in articleCards)
        {
            ArticleRating? rating = userRatings
                .Where(e => e.ArticleId == a.ArticleId)
                .FirstOrDefault();

            if (rating != null)
            {
                a.RatingData.UserRating = rating.Rate;
            }
        }
    }

    public async Task<List<TopArticleItem>> GetTopArticles(int count)
    {
        return (await articleRepository.GetTopArticles(count)).ConvertAll(frontendService.CreateTopArticleItem);
    }

    public async Task<List<TopArticleItem>> GetTopArticlesByTag(int tagId, int count)
    {
        return (await articleRepository.GetTopArticlesByTag(tagId, count)).ConvertAll(frontendService.CreateTopArticleItem);
    }

    public async Task<List<TopArticleItem>> GetTopArticlesByTag(string tagName, int count)
    {
        return (await articleRepository.GetTopArticlesByTag(tagName, count)).ConvertAll(frontendService.CreateTopArticleItem);
    }

    public async Task<List<UserListItem>> GetLatestTagFollowers(List<TagFollow> latestFollowers, int? userId)
    {
        List<UserFollow> followings = new();

        if (userId != null)
        {
            followings = await userRepository.GetFollowings((int)userId);
        }

        return latestFollowers.ConvertAll(tf => new UserListItem
        {
            Link = frontendService.CreateUserLink(tf.User!),
            IsFollowedByCurrentUser = followings.Any(uf => uf.FollowedUserId == tf.UserId)
        });
    }

    public async Task<List<UserListItem>> GetLatestTagFollowers(int tagId, int? userId, int count)
    {
        List<TagFollow> latestFollowers = await tagRepository.GetLatestFollowers(tagId, count);

        return await GetLatestTagFollowers(latestFollowers, userId);
    }

    public async Task<List<UserListItem>> GetLatestTagFollowers(string tagName, int? userId, int count)
    {
        List<TagFollow> latestFollowers = await tagRepository.GetLatestFollowers(tagName, count);

        return await GetLatestTagFollowers(latestFollowers, userId);
    }
}
