using SkillForge.Areas.Admin.Models.DTOs;
using SkillForge.Areas.Admin.Models;
using SkillForge.Models.Database;

namespace SkillForge.Areas.Admin.Repositories;

public interface ICommentRepository : ICrudRepository<Comment>
{
    Task<PaginatedList<Comment>> ListDeleted(ListingModel listingModel, Func<IQueryable<Comment>, IQueryable<Comment>>? queryCallback = null);

    Task<CommentRating?> GetUserRating(int userId, int commentId);

    Task UpsertUserRating(CommentRating rating);

    Task<List<CommentRating>> GetRating(int commentId, int batchIndex, int batchSize, bool positive);

    Task<int> SoftDelete(Comment comment, Violation reason);

    Task<int> SoftDelete(int id, Violation reason);

    Task SoftDelete(int id, CommentReport report);

    Task<int> SoftDeleteMultiple(List<Comment> comments, Violation reason);

    Task<int> SoftDeleteMultiple(List<int> ids, Violation reason);

    Task<int> Restore(Comment comment);

    Task<int> Restore(int id);
}
