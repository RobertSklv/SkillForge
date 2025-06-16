using SkillForge.Areas.Admin.Repositories;
using SkillForge.Models.Database;
using SkillForge.Models.DTOs.Article;
using SkillForge.Models.DTOs.Tag;
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

    public async Task<List<ArticleCard>> GetLatestArticlesByAuthor(string authorName, int? userId, int batchIndex, int batchSize)
    {
        List<ArticleCard> latest = (await articleRepository.GetLatestByAuthor(authorName, batchIndex, batchSize)).ConvertAll(frontendService.CreateArticleCard);

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

    public async Task<List<TopArticleItem>> GetTopArticlesByAuthor(int userId, int count)
    {
        return (await articleRepository.GetTopArticlesByAuthor(userId, count)).ConvertAll(frontendService.CreateTopArticleItem);
    }

    public async Task<List<TagListItem>> CreateTagListItems(List<TagFollow> followers, int? userId)
    {
        List<TagFollow> followings = new();

        if (userId != null)
        {
            List<int> tagIds = followers.ConvertAll(f => f.TagId);

            followings = await userRepository.GetTagFollowings((int)userId, tagIds);
        }

        return followers.ConvertAll(tf => new TagListItem
        {
            Link = frontendService.CreateTagLink(tf.Tag!),
            IsFollowedByCurrentUser = followings.Any(uf => uf.TagId == tf.TagId)
        });
    }

    public async Task<List<UserListItem>> CreateUserListItems<T>(List<T> followEntities, int? userId)
        where T : IFollowEntity
    {
        List<UserFollow> followings = new();

        if (userId != null)
        {
            List<int> userIds = new();

            foreach (T f in followEntities)
            {
                if (f is IFollower follower)
                {
                    userIds.Add(follower.FollowerId);
                }
                if (f is IFollowedUser followedUser)
                {
                    userIds.Add(followedUser.FollowedUserId);
                }
            }

            followings = await userRepository.GetFollowings((int)userId, userIds.Distinct().ToList());
        }

        return followEntities.ConvertAll(fe =>
        {
            if ((typeof(T).IsAssignableFrom(typeof(IFollower)) || typeof(T).Equals(typeof(TagFollow))) && fe is IFollower f)
            {
                return new UserListItem
                {
                    Link = frontendService.CreateUserLink(f.Follower!),
                    IsFollowedByCurrentUser = followings.Any(uf => uf.FollowedUserId == f.FollowerId)
                };
            }
            else if (typeof(T).IsAssignableFrom(typeof(IFollowedUser)) && fe is IFollowedUser fu)
            {
                return new UserListItem
                {
                    Link = frontendService.CreateUserLink(fu.FollowedUser!),
                    IsFollowedByCurrentUser = followings.Any(uf => uf.FollowedUserId == fu.FollowedUserId)
                };
            }
            else throw new Exception("Unexpected outcome");
        });
    }

    public async Task<List<UserListItem>> GetTagFollowers(int tagId, int? userId, int batchIndex, int batchSize)
    {
        List<TagFollow> followers = await tagRepository.GetFollowers(tagId, batchIndex, batchSize);

        return await CreateUserListItems(followers, userId);
    }

    public async Task<List<UserListItem>> GetUserFollowers(int userId, int? currentUserId, int batchIndex, int batchSize)
    {
        List<UserFollow> followers = await userRepository.GetLatestFollowers(userId, batchIndex, batchSize);

        return await CreateUserListItems(followers, currentUserId);
    }

    public async Task<List<UserListItem>> GetUserFollowings(int userId, int? currentUserId, int batchIndex, int batchSize)
    {
        List<UserFollow> followings = await userRepository.GetLatestFollowings(userId, batchIndex, batchSize);

        return await CreateUserListItems(followings, currentUserId);
    }

    public async Task<List<TagListItem>> GetUserTagFollowings(int userId, int? currentUserId, int batchIndex, int batchSize)
    {
        List<TagFollow> followings = await userRepository.GetLatestTagFollowings(userId, batchIndex, batchSize);

        return await CreateTagListItems(followings, currentUserId);
    }

    public async Task<List<UserListItem>> GetLatestTagFollowers(int tagId, int? userId, int count)
    {
        List<TagFollow> latestFollowers = await tagRepository.GetLatestFollowers(tagId, count);

        return await CreateUserListItems(latestFollowers, userId);
    }

    public async Task<List<UserListItem>> GetLatestTagFollowers(string tagName, int? userId, int count)
    {
        List<TagFollow> latestFollowers = await tagRepository.GetLatestFollowers(tagName, count);

        return await CreateUserListItems(latestFollowers, userId);
    }

    public async Task<List<UserListItem>> GetLatestUserFollowers(int userId, int? currentUserId, int count)
    {
        List<UserFollow> latestFollowers = await userRepository.GetLatestFollowers(userId, 0, count);

        return await CreateUserListItems(latestFollowers.ConvertAll(f => f as IFollower), currentUserId);
    }

    public async Task<List<UserListItem>> GetLatestUserFollowings(int userId, int? currentUserId, int count)
    {
        List<UserFollow> latestFollowings = await userRepository.GetLatestFollowings(userId, 0, count);

        return await CreateUserListItems(latestFollowings.ConvertAll(f => f as IFollowedUser), currentUserId);
    }

    public async Task<List<TagListItem>> GetLatestUserTagFollowings(int userId, int? currentUserId, int count)
    {
        List<TagFollow> latestFollowings = await userRepository.GetLatestTagFollowings(userId, 0, count);

        return await CreateTagListItems(latestFollowings, currentUserId);
    }
}
