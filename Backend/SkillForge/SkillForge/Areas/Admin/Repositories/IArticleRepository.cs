using SkillForge.Areas.Admin.Models.DTOs;
using SkillForge.Areas.Admin.Models;
using SkillForge.Models.Database;

namespace SkillForge.Areas.Admin.Repositories;

public interface IArticleRepository : ICrudRepository<Article>
{
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

    Task<PaginatedList<Article>> ListByTag(ListingModel listingModel, int tagId);

    Task<List<Article>> GetLatest(int batchIndex, int batchSize);
}
