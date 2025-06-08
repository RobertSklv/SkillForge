using SkillForge.Models.Database;
using SkillForge.Models.DTOs.Article;
using SkillForge.Models.DTOs.User;

namespace SkillForge.Services;

public interface IUserFeedService
{
    Task<List<ArticleCard>> GetLatestArticles(int? userId, int batchIndex, int batchSize);

    Task<List<ArticleCard>> GetLatestArticlesByTag(int tagId, int? userId, int batchIndex, int batchSize);

    Task<List<ArticleCard>> GetLatestArticlesByTag(string tagName, int? userId, int batchIndex, int batchSize);

    Task SetCurrentUserRating(List<ArticleCard> articleCards, int userId);

    Task<List<TopArticleItem>> GetTopArticles(int count);

    Task<List<TopArticleItem>> GetTopArticlesByTag(int tagId, int count);

    Task<List<TopArticleItem>> GetTopArticlesByTag(string tagName, int count);

    Task<List<UserListItem>> GetTagFollowers(List<TagFollow> followers, int? userId);

    Task<List<UserListItem>> GetTagFollowers(int tagId, int? userId, int batchIndex, int batchSize);

    Task<List<UserListItem>> GetLatestTagFollowers(int tagId, int? userId, int count);

    Task<List<UserListItem>> GetLatestTagFollowers(string tagName, int? userId, int count);
}