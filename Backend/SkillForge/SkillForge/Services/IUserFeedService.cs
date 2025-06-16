using SkillForge.Models.Database;
using SkillForge.Models.DTOs.Article;
using SkillForge.Models.DTOs.Tag;
using SkillForge.Models.DTOs.User;

namespace SkillForge.Services;

public interface IUserFeedService
{
    Task<List<ArticleCard>> GetLatestArticles(int? userId, int batchIndex, int batchSize);

    Task<List<ArticleCard>> GetLatestArticlesByTag(int tagId, int? userId, int batchIndex, int batchSize);

    Task<List<ArticleCard>> GetLatestArticlesByTag(string tagName, int? userId, int batchIndex, int batchSize);

    Task<List<ArticleCard>> GetLatestArticlesByAuthor(string authorName, int? userId, int batchIndex, int batchSize);

    Task SetCurrentUserRating(List<ArticleCard> articleCards, int userId);

    Task<List<TopArticleItem>> GetTopArticles(int count);

    Task<List<TopArticleItem>> GetTopArticlesByTag(int tagId, int count);

    Task<List<TopArticleItem>> GetTopArticlesByTag(string tagName, int count);

    Task<List<TopArticleItem>> GetTopArticlesByAuthor(int userId, int count);

    Task<List<TagListItem>> CreateTagListItems(List<TagFollow> followers, int? userId);

    Task<List<UserListItem>> CreateUserListItems(List<User> users, int? userId);

    Task<List<UserListItem>> CreateUserListItems<T>(List<T> followEntities, int? userId) where T : IFollowEntity;

    Task<List<UserListItem>> GetTagFollowers(int tagId, int? userId, int batchIndex, int batchSize);

    Task<List<UserListItem>> GetUserFollowers(int userId, int? currentUserId, int batchIndex, int batchSize);

    Task<List<UserListItem>> GetUserFollowings(int userId, int? currentUserId, int batchIndex, int batchSize);

    Task<List<TagListItem>> GetUserTagFollowings(int userId, int? currentUserId, int batchIndex, int batchSize);

    Task<List<UserListItem>> GetLatestTagFollowers(int tagId, int? userId, int count);

    Task<List<UserListItem>> GetLatestTagFollowers(string tagName, int? userId, int count);

    Task<List<UserListItem>> GetLatestUserFollowers(int userId, int? currentUserId, int count);

    Task<List<UserListItem>> GetLatestUserFollowings(int userId, int? currentUserId, int count);

    Task<List<TagListItem>> GetLatestUserTagFollowings(int userId, int? currentUserId, int count);
}