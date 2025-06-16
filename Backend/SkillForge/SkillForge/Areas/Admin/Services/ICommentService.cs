using SkillForge.Models.Database;
using SkillForge.Models.DTOs.Article;
using SkillForge.Models.DTOs.Rating;
using SkillForge.Models.DTOs.User;

namespace SkillForge.Areas.Admin.Services;

public interface ICommentService : ICrudService<Comment>
{
    Task<Comment> Add(int userId, int articleId, string content);

    Task Rate(int userId, int commentId, UserRatingData rate);

    Task<CommentRating?> GetUserRating(int userId, int commentId);

    Task<List<UserListItem>> GetRating(int articleId, int? currentUserId, int batchIndex, int batchSize, bool positive);
}
