using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using SkillForge.Areas.Admin.Models;
using SkillForge.Areas.Admin.Models.Components.Grid;
using SkillForge.Areas.Admin.Models.DTOs;
using SkillForge.Models.DTOs.Article;
using SkillForge.Models.DTOs.Rating;
using SkillForge.Areas.Admin.Repositories;
using SkillForge.Attributes;
using SkillForge.Models.Database;
using SkillForge.Models.DTOs.User;
using SkillForge.Services;
using SkillForge.Models.DTOs.Search;
using SkillForge.Exceptions;

namespace SkillForge.Areas.Admin.Services;

public class ArticleService : CrudService<Article>, IArticleService
{
    private readonly IArticleRepository repository;
    private readonly ITagService tagService;
    private readonly IArticleTagMtmRepository articleTagRepository;
    private readonly IFrontendService frontendService;
    private readonly IUserFeedService userFeedService;
    private readonly IUserService userService;
    private readonly IArticleReportService articleReportService;

    public ArticleService(
        IArticleRepository repository,
        ITagService tagService,
        IArticleTagMtmRepository articleTagRepository,
        IFrontendService frontendService,
        IUserFeedService userFeedService,
        IUserService userService,
        IArticleReportService articleReportService)
        : base(repository)
    {
        this.repository = repository;
        this.tagService = tagService;
        this.articleTagRepository = articleTagRepository;
        this.frontendService = frontendService;
        this.userFeedService = userFeedService;
        this.userService = userService;
        this.articleReportService = articleReportService;
    }

    public override Table<Article> CreateEditRowAction(Table<Article> table)
    {
        // Creates a View action instead of Edit.

        return table.AddRowAction("View");
    }

    public override Table<Article> CreateDeleteRowAction(Table<Article> table)
    {
        return table;
    }

    public override async Task<Table<Article>> CreateListingTable(ListingModel<Article> listingModel, PaginatedList<Article> items)
    {
        return (await base.CreateListingTable(listingModel, items))
            .RemoveColumn(nameof(Article.DeleteReason));
    }

    public async Task<ListingModel<Article>> CreatePendingArticlesListing(ListingModel listingQuery)
    {
        ListingModel<Article> model = new();
        model = InitializeListingModel(model, listingQuery);
        model.ActionName = "Pending";

        PaginatedList<Article> items = await repository.ListPending(model);

        model.Table = new Table<Article>(model, items)
            .AddRowAction("Preview")
            .AddMassAction("MassApprove", "Approve selected")
            .RemoveColumn(nameof(Article.DeleteReason))
            .SetAdjustablePageSize(true)
            .SetFilterable(true)
            .SetOrderable(true)
            .SetSearchable(true)
            .AddPagination(true);

        return model;
    }

    public async Task<ListingModel<Article>> CreateDeletedArticlesListing(ListingModel listingQuery)
    {
        ListingModel<Article> model = new();
        model = InitializeListingModel(model, listingQuery);
        model.ActionName = "Deleted";

        PaginatedList<Article> items = await repository.ListDeleted(model);

        model.Table = new Table<Article>(model, items)
            .AddRowAction("View")
            .SetAdjustablePageSize(true)
            .SetFilterable(true)
            .SetOrderable(true)
            .SetSearchable(true)
            .AddPagination(true);

        return model;
    }

    public async Task<ListingModel<Article>> CreateListingByTag(ListingModel listingQuery, int tagId)
    {
        ListingModel<Article> listing = new();
        listing.CopyFrom(listingQuery);

        PaginatedList<Article> items = await ListByTag(listing, tagId);

        listing.Table = new Table<Article>(listing, items)
            .AddRowActionGeneric("View", customizationCallback: e => e.SetRoute(article =>
                $"/Admin/Article/View/{article.Id}"))
            .RemoveColumn(nameof(Article.DeleteReason))
            .AddPagination(true)
            .SetOrderable(true)
            .SetSearchable(true)
            .SetFilterable(true);

        return listing;
    }

    public Task<PaginatedList<Article>> ListByTag(ListingModel listingModel, int tagId)
    {
        return repository.ListByTag(listingModel, tagId);
    }

    public async Task<bool> SoftDelete(Article article, Violation reason)
    {
        return await repository.SoftDelete(article, reason) > 0;
    }

    public async Task<bool> SoftDelete(int id, Violation reason)
    {
        return await repository.SoftDelete(id, reason) > 0;
    }

    public Task SoftDelete(int id, ArticleReport report)
    {
        return repository.SoftDelete(id, report);
    }

    public async Task SoftDelete(int id, int articleReportId)
    {
        ArticleReport report = await articleReportService.GetStrict(articleReportId);

        await SoftDelete(id, report);
    }

    public async Task<bool> SoftDeleteMultiple(List<Article> articles, Violation reason)
    {
        return await repository.SoftDeleteMultiple(articles, reason) > 0;
    }

    public async Task<bool> SoftDeleteMultiple(List<int> ids, Violation reason)
    {
        return await repository.SoftDeleteMultiple(ids, reason) > 0;
    }

    public async Task<bool> Restore(Article article)
    {
        return await repository.Restore(article) > 0;
    }

    public async Task<bool> Restore(int id)
    {
        return await repository.Restore(id) > 0;
    }

    public void CreateArticleApproval(Article article, int adminId)
    {
        ArticleApproval approval = new()
        {
            ModeratorId = adminId
        };

        article.Approval = approval;
        article.Author!.ArticlesCount++;
    }

    public async Task<bool> Approve(int id, int adminId)
    {
        Article article = await GetStrict(id);

        CreateArticleApproval(article, adminId);

        return await Upsert(article);
    }

    public async Task<bool> MassApprove(List<int> ids, int adminId)
    {
        List<Article> articles = await GetByIds(ids);

        foreach (Article a in articles)
        {
            CreateArticleApproval(a, adminId);
        }

        return await UpsertMultiple(articles);
    }

    public bool ValidateTagNames(List<string> tags, string propertyName, ModelStateDictionary modelState)
    {
        Regex r = new(CodeAttribute.PATTERN);
        List<string> invalidTags = tags
            .Where(tag => !r.IsMatch(tag))
            .ToList();

        if (invalidTags.Any())
        {
            modelState.AddModelError(
                propertyName,
                $"Invalid tag(s). The following tags contain invalid characters: " + string.Join(", ", invalidTags));

            return false;
        }

        return true;
    }

    public async Task UserUpsert(ArticleUpsertDTO model, int userId)
    {
        Article entity = await Get(model.Id) ?? new();

        if (entity.DeleteReason != null)
        {
            throw new RecordDeletedException("The article is deleted");
        }

        if (entity.Id != 0 && userId != entity.AuthorId)
        {
            throw new NotOwnedByUserException("The article is not owned by the current user.");
        }

        entity.Id = model.Id;
        entity.AuthorId = userId;
        entity.Image = model.Image;
        entity.Title = model.Title;
        entity.Content = model.Content;

        await repository.Upsert(entity);

        List<Tag> tags = await tagService.GetByNamesAndCreateNonexisting(model.Tags);

        tags.ForEach(t => t.ArticlesCount++);

        List<int> tagIds = tags.ConvertAll(t => t.Id).ToList();

        await articleTagRepository.UpdateLeft(entity.Id, tagIds);
    }

    public async Task<ArticlePageData> View(int userId, int articleId)
    {
        Article article = await GetWithComments(articleId);
        
        if (article.DeleteReason != null)
        {
            throw new RecordDeletedException("The article is deleted");
        }

        RegisteredArticleView? viewRecord = await GetView(userId, articleId);

        if (viewRecord == null)
        {
            viewRecord = new RegisteredArticleView
            {
                UserId = userId,
                ArticleId = articleId,
            };

            article.ViewCount++;

            await RecordView(viewRecord);
        }

        List<Article> latestByAuthor = await repository.GetLatestByAuthorExcluding(article.AuthorId, articleId, 3);
        bool isAuthorFollowedByCurrentUser = await userService.IsFollowedBy(article.AuthorId, userId);
        ArticlePageData model = frontendService.CreateArticlePageData(article, isAuthorFollowedByCurrentUser, latestByAuthor);
        ArticleRating? userRating = await GetUserRating(userId, articleId);
        List<int> commentIds = model.Comments.ConvertAll(c => c.CommentId);
        List<CommentRating> commentUserRatings = await GetUserCommentRating(userId, commentIds);

        if (userRating != null)
        {
            model.RatingData.UserRating = userRating.Rate;
        }

        foreach (CommentModel comment in model.Comments)
        {
            CommentRating? rating = commentUserRatings
                .Where(e => e.CommentId == comment.CommentId)
                .FirstOrDefault();

            if (rating != null)
            {
                comment.RatingData.UserRating = rating.Rate;
            }
        }

        return model;
    }

    public async Task<ArticlePageData> View(string guestId, int articleId)
    {
        Article article = await GetWithComments(articleId);

        if (article.DeleteReason != null)
        {
            throw new RecordDeletedException("The article is deleted");
        }

        GuestArticleView? viewRecord = await GetView(guestId, articleId);

        if (viewRecord == null)
        {
            viewRecord = new GuestArticleView
            {
                GuestId = guestId,
                ArticleId = articleId,
            };

            article.ViewCount++;

            await RecordView(viewRecord);
        }

        List<Article> latestByAuthor = await repository.GetLatestByAuthorExcluding(article.AuthorId, articleId, 3);
        ArticlePageData model = frontendService.CreateArticlePageData(article, false, latestByAuthor);

        return model;
    }

    public async Task<ArticleUpsertPageModel> LoadUpsertPage(int? id, int userId)
    {
        ArticleUpsertPageModel pageModel = new();

        if (id != null)
        {
            Article article = await GetStrict((int)id);

            if (article.DeleteReason != null)
            {
                throw new RecordDeletedException("The article is deleted");
            }

            if (article.Id != 0 && userId != article.AuthorId)
            {
                throw new NotOwnedByUserException("The article is not owned by the current user.");
            }

            pageModel.CurrentState = new ArticleState
            {
                Model = new ArticleUpsertDTO
                {
                    Id = article.Id,
                    Image = article.Image,
                    Title = article.Title,
                    Content = article.Content,
                    Tags = article.Tags!.ConvertAll(t => t.Tag!.Name).ToList(),
                },
                IsApproved = article.ApprovalId != null
            };
        }

        return pageModel;
    }

    public async Task Rate(int userId, int articleId, UserRatingData rate)
    {
        ArticleRating? rating = await GetUserRating(userId, articleId);

        short oldRate = 0;

        if (rating != null)
        {
            oldRate = rating.Rate;
        }

        rating ??= new ArticleRating()
        {
            UserId = userId,
            ArticleId = articleId,
        };

        rating.Rate = Math.Clamp(rate.Rate, (short)-1, (short)1);

        Article article = await GetStrict(articleId);

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

    public Task<Article> GetWithComments(int id)
    {
        return repository.GetWithComments(id);
    }

    public Task<ArticleRating?> GetUserRating(int userId, int articleId)
    {
        return repository.GetUserRating(userId, articleId);
    }

    public Task<List<ArticleRating>> GetUserRating(int userId, List<int> articleIds)
    {
        return repository.GetUserRating(userId, articleIds);
    }

    public Task<List<CommentRating>> GetUserCommentRating(int userId, List<int> commentIds)
    {
        return repository.GetUserCommentRating(userId, commentIds);
    }

    public Task<RegisteredArticleView?> GetView(int userId, int articleId)
    {
        return repository.GetView(userId, articleId);
    }

    public Task<GuestArticleView?> GetView(string guestId, int articleId)
    {
        return repository.GetView(guestId, articleId);
    }

    public Task RecordView(RegisteredArticleView view)
    {
        return repository.RecordView(view);
    }

    public Task RecordView(GuestArticleView view)
    {
        return repository.RecordView(view);
    }

    public Task<List<Article>> Search(string phrase)
    {
        return repository.Search(phrase);
    }

    public async Task<List<ArticleSearchItem>> SearchItems(string phrase)
    {
        return (await repository.Search(phrase)).ConvertAll(frontendService.CreateArticleSearchItem);
    }

    public async Task<PaginationResponse<ArticleCard>> SearchAdvancedCards(GridState gridState)
    {
        if (gridState.SortBy == "date")
        {
            gridState.SortBy = nameof(BaseEntity.CreatedAt);
        }
        else if (gridState.SortBy == "rating")
        {
            gridState.SortBy = nameof(Article.ThumbsUp);
        }
        else if (gridState.SortBy == "views")
        {
            gridState.SortBy = nameof(Article.ViewCount);
        }

        ListingModel listingQuery = new()
        {
            Page = gridState.P ?? 1,
            PageSize = gridState.Limit ?? 9,
            SearchPhrase = gridState.Q,
            OrderBy = gridState.SortBy ?? nameof(BaseEntity.CreatedAt),
            Direction = gridState.SortOrder ?? "desc",
        };

        PaginatedList<Article> list = await List(listingQuery);

        PaginationResponse<ArticleCard> response = new()
        {
            Items = list.ConvertAll(frontendService.CreateArticleCard),
            ItemCount = list.Count,
            TotalItems = list.TotalItems,
        };

        return response;
    }

    public async Task<List<UserListItem>> GetRating(int articleId, int? currentUserId, int batchIndex, int batchSize, bool positive)
    {
        List<ArticleRating> ratings = await repository.GetRating(articleId, batchIndex, batchSize, positive);

        return await userFeedService.CreateUserListItems(ratings.ConvertAll(rating => rating.User!), currentUserId);
    }
}
