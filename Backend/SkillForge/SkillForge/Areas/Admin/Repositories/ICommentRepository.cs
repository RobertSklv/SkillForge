using SkillForge.Models.Database;

namespace SkillForge.Areas.Admin.Repositories;

public interface ICommentRepository : ICrudRepository<Comment>
{
    Task<CommentRating?> GetUserRating(int userId, int commentId);

    Task UpsertUserRating(CommentRating rating);

    Task<List<CommentRating>> GetRating(int commentId, int batchIndex, int batchSize, bool positive);
}
