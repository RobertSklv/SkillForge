using SkillForge.Models.Database;

namespace SkillForge.Areas.Admin.Repositories;

public interface ICommentRepository : ICrudRepository<Comment>
{
    Task<CommentRating?> GetUserRating(int userId, int commentId);

    Task UpsertUserRating(CommentRating rating);
}
