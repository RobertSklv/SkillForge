using SkillForge.Areas.Admin.Models.DTOs;
using SkillForge.Models.Database;
using SkillForge.Models.DTOs.Article;
using SkillForge.Models.DTOs.Comment;
using SkillForge.Models.DTOs.Rating;
using SkillForge.Models.DTOs.User;

namespace SkillForge.Areas.Admin.Services;

public interface ICommentService : ICrudService<Comment>
{
    Task<ListingModel<Comment>> CreateDeletedCommentsListing(ListingModel listingQuery);

    Task<CommentModel> Upsert(int userId, CommentUpsertFormData formData);

    Task Rate(int userId, int commentId, UserRatingData rate);

    Task<CommentRating?> GetUserRating(int userId, int commentId);

    Task<List<UserListItem>> GetRating(int articleId, int? currentUserId, int batchIndex, int batchSize, bool positive);

    Task<bool> SoftDelete(Comment comment, Violation reason);

    Task<bool> SoftDelete(int id, Violation reason);

    Task SoftDelete(int id, CommentReport report);

    Task SoftDelete(int id, int commentReportId);

    Task<bool> SoftDeleteMultiple(List<Comment> comments, Violation reason);

    Task<bool> SoftDeleteMultiple(List<int> ids, Violation reason);

    Task<bool> Restore(Comment comment);

    Task<bool> Restore(int id);
}
