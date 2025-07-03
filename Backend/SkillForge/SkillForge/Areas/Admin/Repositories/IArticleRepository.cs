using SkillForge.Areas.Admin.Models.DTOs;
using SkillForge.Areas.Admin.Models;
using SkillForge.Models.Database;

namespace SkillForge.Areas.Admin.Repositories;

public interface IArticleRepository : ICrudRepository<Article>
{
    Task<int> SoftDelete(Article article, Violation reason);

    Task<int> SoftDelete(int id, Violation reason);

    Task SoftDelete(int id, ArticleReport report);

    Task<int> SoftDeleteMultiple(List<Article> articles, Violation reason);

    Task<int> SoftDeleteMultiple(List<int> ids, Violation reason);

    Task<int> Restore(Article article);

    Task<int> Restore(int id);

    Task ResetApproval(int approvalId);

    Task<Article> GetWithComments(int id);

    Task<ArticleRating?> GetUserRating(int userId, int articleId);

    Task<List<ArticleRating>> GetUserRating(int userId, List<int> articleIds);

    Task<List<CommentRating>> GetUserCommentRating(int userId, List<int> commentIds);

    Task UpsertUserRating(ArticleRating rating);

    Task<RegisteredArticleView?> GetView(int userId, int articleId);

    Task<GuestArticleView?> GetView(string guestId, int articleId);

    Task RecordView(RegisteredArticleView view);

    Task RecordView(GuestArticleView view);

    Task<PaginatedList<Article>> ListPending(ListingModel listingModel, Func<IQueryable<Article>, IQueryable<Article>>? queryCallback = null);

    Task<PaginatedList<Article>> ListDeleted(ListingModel listingModel, Func<IQueryable<Article>, IQueryable<Article>>? queryCallback = null);

    Task<PaginatedList<Article>> ListByTag(ListingModel listingModel, int tagId);

    Task<List<Article>> GetLatest(int batchIndex, int batchSize);

    Task<List<Article>> GetLatestByTag(int tagId, int batchIndex, int batchSize);

    Task<List<Article>> GetLatestByTag(string tagName, int batchIndex, int batchSize);

    Task<List<Article>> GetLatestByAuthor(string authorName, int batchIndex, int batchSize);

    Task<List<Article>> GetLatestByAuthorExcluding(int authorId, int excludedArticleId, int count);

    Task<List<ArticleRating>> GetRating(int articleId, int batchIndex, int batchSize, bool positive);

    Task<List<Article>> GetTopArticlesByTag(int tagId, int count);

    Task<List<Article>> GetTopArticlesByTag(string tagName, int count);

    Task<List<Article>> GetTopArticlesByAuthor(int authorId, int count);

    Task<List<Article>> GetTopArticles(int count);

    Task<List<Article>> Search(string phrase);
}
