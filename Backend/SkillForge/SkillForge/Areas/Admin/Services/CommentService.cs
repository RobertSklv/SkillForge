using SkillForge.Areas.Admin.Repositories;
using SkillForge.Models.Database;
using SkillForge.Models.DTOs.Rating;
using SkillForge.Models.DTOs.User;
using SkillForge.Services;

namespace SkillForge.Areas.Admin.Services;

public class CommentService : CrudService<Comment>, ICommentService
{
    private readonly ICommentRepository repository;
    private readonly IUserFeedService userFeedService;
    private readonly ICommentReportService commentReportService;

    public CommentService(ICommentRepository repository, IUserFeedService userFeedService, ICommentReportService commentReportService)
        : base(repository)
    {
        this.repository = repository;
        this.userFeedService = userFeedService;
        this.commentReportService = commentReportService;
    }

    public async Task<Comment> Add(int userId, int articleId, string content)
    {
        Comment comment = new()
        {
            UserId = userId,
            ArticleId = articleId,
            Content = content,
        };

        await Upsert(comment);

        return comment;
    }

    public async Task Rate(int userId, int commentId, UserRatingData rate)
    {
        CommentRating? rating = await GetUserRating(userId, commentId);

        short oldRate = 0;

        if (rating != null)
        {
            oldRate = rating.Rate;
        }

        rating ??= new CommentRating()
        {
            UserId = userId,
            CommentId = commentId,
        };

        rating.Rate = Math.Clamp(rate.Rate, (short)-1, (short)1);

        Comment article = await GetStrict(commentId);

        if (rating.Rate != oldRate)
        {
            if (oldRate > 0)
            {
                article.ThumbsUp--;
            }
            else if (oldRate < 0)
            {
                article.ThumbsDown--;
            }

            if (rating.Rate > 0)
            {
                article.ThumbsUp++;
            }
            else if (rating.Rate < 0)
            {
                article.ThumbsDown++;
            }
        }

        await repository.UpsertUserRating(rating);
    }

    public Task<CommentRating?> GetUserRating(int userId, int commentId)
    {
        return repository.GetUserRating(userId, commentId);
    }

    public async Task<List<UserListItem>> GetRating(int articleId, int? currentUserId, int batchIndex, int batchSize, bool positive)
    {
        List<CommentRating> ratings = await repository.GetRating(articleId, batchIndex, batchSize, positive);

        return await userFeedService.CreateUserListItems(ratings.ConvertAll(rating => rating.User!), currentUserId);
    }

    public async Task<bool> SoftDelete(Comment comment, Violation reason)
    {
        return await repository.SoftDelete(comment, reason) > 0;
    }

    public async Task<bool> SoftDelete(int id, Violation reason)
    {
        return await repository.SoftDelete(id, reason) > 0;
    }

    public Task SoftDelete(int id, CommentReport report)
    {
        return repository.SoftDelete(id, report);
    }

    public async Task SoftDelete(int id, int commentReportId)
    {
        CommentReport report = await commentReportService.GetStrict(commentReportId);

        await SoftDelete(id, report);
    }

    public async Task<bool> SoftDeleteMultiple(List<Comment> comments, Violation reason)
    {
        return await repository.SoftDeleteMultiple(comments, reason) > 0;
    }

    public async Task<bool> SoftDeleteMultiple(List<int> ids, Violation reason)
    {
        return await repository.SoftDeleteMultiple(ids, reason) > 0;
    }

    public async Task<bool> Restore(Comment comment)
    {
        return await repository.Restore(comment) > 0;
    }

    public async Task<bool> Restore(int id)
    {
        return await repository.Restore(id) > 0;
    }
}
