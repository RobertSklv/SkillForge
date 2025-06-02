using SkillForge.Areas.Admin.Models.DTOs.Article;
using SkillForge.Areas.Admin.Models.DTOs.Rating;
using SkillForge.Models.Database;

namespace SkillForge.Areas.Admin.Services;

public interface ICommentService : ICrudService<Comment>
{
    Task<Comment> Add(int userId, int articleId, string content);

    Task Rate(int userId, int commentId, UserRatingData rate);

    Task<CommentRating?> GetUserRating(int userId, int commentId);

    CommentModel CreateCommentModel(Comment comment);
}
