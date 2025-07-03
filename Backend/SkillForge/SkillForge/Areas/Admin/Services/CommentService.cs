using SkillForge.Areas.Admin.Models.Components.Grid;
using SkillForge.Areas.Admin.Models.DTOs;
using SkillForge.Areas.Admin.Models;
using SkillForge.Areas.Admin.Repositories;
using SkillForge.Exceptions;
using SkillForge.Models.Database;
using SkillForge.Models.DTOs.Article;
using SkillForge.Models.DTOs.Comment;
using SkillForge.Models.DTOs.Rating;
using SkillForge.Models.DTOs.User;
using SkillForge.Services;

namespace SkillForge.Areas.Admin.Services;

public class CommentService : CrudService<Comment>, ICommentService
{
    private readonly ICommentRepository repository;
    private readonly IUserService userService;
    private readonly IUserFeedService userFeedService;
    private readonly ICommentReportService commentReportService;
    private readonly IFrontendService frontendService;
    private readonly IArticleService articleService;

    public CommentService(
        ICommentRepository repository,
        IUserService userService,
        IUserFeedService userFeedService,
        ICommentReportService commentReportService,
        IFrontendService frontendService,
        IArticleService articleService)
        : base(repository)
    {
        this.repository = repository;
        this.userService = userService;
        this.userFeedService = userFeedService;
        this.commentReportService = commentReportService;
        this.frontendService = frontendService;
        this.articleService = articleService;
    }

    public override Table<Comment> CreateEditRowAction(Table<Comment> table)
    {
        // Creates a View action instead of Edit.

        return table.AddRowAction("View");
    }

    public override Table<Comment> CreateDeleteRowAction(Table<Comment> table)
    {
        return table;
    }

    public override async Task<Table<Comment>> CreateListingTable(ListingModel<Comment> listingModel, PaginatedList<Comment> items)
    {
        return (await base.CreateListingTable(listingModel, items))
            .RemoveColumn(nameof(Comment.DeleteReason))
            .RemoveColumn(nameof(Comment.UpdatedAt))
            .SetSelectableOptionsSource(nameof(Comment.User), await userService.GetAll())
            .SetSelectableOptionsSource(nameof(Comment.Article), await articleService.GetAll());
    }

    public async Task<ListingModel<Comment>> CreateDeletedCommentsListing(ListingModel listingQuery)
    {
        ListingModel<Comment> model = new();
        model = InitializeListingModel(model, listingQuery);
        model.ActionName = "Deleted";

        PaginatedList<Comment> items = await repository.ListDeleted(model);

        model.Table = new Table<Comment>(model, items)
            .AddRowAction("View")
            .SetAdjustablePageSize(true)
            .SetFilterable(true)
            .SetOrderable(true)
            .SetSearchable(true)
            .AddPagination(true);

        return model;
    }

    public async Task<CommentModel> Upsert(int userId, CommentUpsertFormData formData)
    {
        Comment? existing = await Get(formData.CommentId);

        if (existing != null)
        {
            existing.ContentEditedAt = DateTime.Now;

            if (existing.UserId != userId)
            {
                throw new NotOwnedByUserException("Unauthorized to edit comment");
            }
        }

        Comment comment = existing ?? new()
        {
            UserId = userId,
            ArticleId = formData.ArticleId ?? throw new Exception("ArticleId parameter is required"),
        };

        if (existing == null)
        {
            comment.User = await userService.GetStrict(userId);
        }

        comment.Content = formData.Content;

        await Upsert(comment);

        CommentModel model = frontendService.CreateCommentModel(comment);

        CommentRating? rating = await GetUserRating(userId, formData.CommentId);

        if (rating != null)
        {
            model.RatingData.UserRating = rating.Rate;
        }

        return model;
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
