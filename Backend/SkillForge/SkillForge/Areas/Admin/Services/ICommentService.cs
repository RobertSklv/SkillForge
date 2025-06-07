using SkillForge.Models.Database;
using SkillForge.Models.DTOs.Article;
using SkillForge.Models.DTOs.Rating;

namespace SkillForge.Areas.Admin.Services;

public interface ICommentService : ICrudService<Comment>
{
    Task<Comment> Add(int userId, int articleId, string content);

    Task Rate(int userId, int commentId, UserRatingData rate);

    Task<CommentRating?> GetUserRating(int userId, int commentId);
}
