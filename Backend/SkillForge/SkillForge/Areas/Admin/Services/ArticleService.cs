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

namespace SkillForge.Areas.Admin.Services;

public class ArticleService : CrudService<Article>, IArticleService
{
    private readonly IArticleRepository repository;
    private readonly ITagService tagService;
    private readonly IArticleTagMtmRepository articleTagRepository;
    private readonly IFrontendService frontendService;

    public ArticleService(
        IArticleRepository repository,
        ITagService tagService,
        IArticleTagMtmRepository articleTagRepository,
        IFrontendService frontendService)
        : base(repository)
    {
        this.repository = repository;
        this.tagService = tagService;
        this.articleTagRepository = articleTagRepository;
        this.frontendService = frontendService;
    }

    public override Table<Article> CreateEditRowAction(Table<Article> table)
    {
        // Creates a View action instead of Edit.

        return table.AddRowAction("View");
    }

    public virtual async Task<ListingModel<Article>> CreatePendingArticlesListing(ListingModel listingQuery)
    {
        ListingModel<Article> model = new();
        model = InitializeListingModel(model, listingQuery);
        model.ActionName = "Pending";

        PaginatedList<Article> items = await repository.ListPending(model);

        model.Table = new Table<Article>(model, items)
            .AddRowAction("Preview")
            .AddChainCall(CreateDeleteRowAction)
            .AddMassAction("MassApprove", "Approve selected")
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
        Article entity = new()
        {
            Id = model.Id,
            CategoryId = model.CategoryId,
            AuthorId = userId,
            Image = model.Image,
            Title = model.Title,
            Content = model.Content,
        };

        await repository.Upsert(entity);

        List<Tag> tags = await tagService.GetByNamesAndCreateNonexisting(model.Tags);

        tags.ForEach(t => t.ArticlesCount++);

        List<int> tagIds = tags.ConvertAll(t => t.Id).ToList();

        await articleTagRepository.UpdateLeft(entity.Id, tagIds);
    }

    public async Task<ArticlePageData> View(int userId, int articleId)
    {
        Article article = await GetWithComments(articleId);
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

        ArticlePageData model = frontendService.CreateArticlePageData(article);
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

        ArticlePageData model = frontendService.CreateArticlePageData(article);

        return model;
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
}
