using SkillForge.Areas.Admin.Models;
using SkillForge.Areas.Admin.Models.Components.Grid;
using SkillForge.Areas.Admin.Models.DTOs;
using SkillForge.Areas.Admin.Models.DTOs.Article;
using SkillForge.Areas.Admin.Models.DTOs.Rating;
using SkillForge.Areas.Admin.Repositories;
using SkillForge.Models.Database;

namespace SkillForge.Areas.Admin.Services;

public class ArticleService : CrudService<Article>, IArticleService
{
    private readonly IArticleRepository repository;
    private readonly ICommentService commentService;
    private readonly ITagService tagService;

    public ArticleService(IArticleRepository repository, ICommentService commentService, ITagService tagService)
        : base(repository)
    {
        this.repository = repository;
        this.commentService = commentService;
        this.tagService = tagService;
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

    public async Task<bool> Approve(int id, int adminId)
    {
        Article article = await GetStrict(id);

        ArticleApproval approval = new()
        {
            ModeratorId = adminId
        };

        article.Approval = approval;

        return await Upsert(article);
    }

    public async Task<bool> MassApprove(List<int> ids, int adminId)
    {
        List<Article> articles = await GetByIds(ids);

        foreach (Article a in articles)
        {
            ArticleApproval approval = new()
            {
                ModeratorId = adminId
            };

            a.Approval = approval;
        }

        return await UpsertMultiple(articles);
    }

    public async Task UserUpsert(ArticleUpsertDTO model, int userId)
    {
        List<Tag> tags = await tagService.GetByNames(model.Tags);

        Article entity = new()
        {
            Id = model.Id,
            CategoryId = model.CategoryId,
            AuthorId = userId,
            Image = model.Image,
            Title = model.Title,
            Content = model.Content,
            Tags = model.Tags.ConvertAll(tagName =>
            {
                Tag? t = tags.Find(tt => tt.Name == tagName);

                ArticleTag at = new()
                {
                    ArticleId = model.Id,
                };

                if (t != null)
                {
                    at.TagId = t.Id;
                }
                else
                {
                    at.Tag = new Tag
                    {
                        Name = tagName
                    };
                }

                return at;
            }).ToList()
        };

        await repository.Upsert(entity);
    }

    public ArticleCard CreateArticleCard(Article article)
    {
        return new ArticleCard()
        {
            Author = new UserLink()
            {
                Id = article.Author!.Id,
                Name = article.Author.Name,
                AvatarImage = article.Author.AvatarPath,
            },
            ArticleId = article.Id,
            Title = article.Title,
            CoverImage = article.Image,
            CategoryCode = article.Category!.Code,
            CategoryName = article.Category.DisplayedName,
            DatePublished = (DateTime)article.CreatedAt!,
            RatingData = new RatingData
            {
                ThumbsUp = article.ThumbsUp,
                ThumbsDown = article.ThumbsDown,
                UserRating = 0,
            },
            Comments = article.Comments!.ConvertAll(commentService.CreateCommentModel),
            Tags = article.Tags!.ConvertAll(at => tagService.CreateTagLink(at.Tag!))
        };
    }

    public TopArticleItem CreateTopArticleItem(Article article)
    {
        return new TopArticleItem()
        {
            Author = new UserLink()
            {
                Id = article.Author!.Id,
                Name = article.Author.Name,
                AvatarImage = article.Author.AvatarPath,
            },
            ArticleId = article.Id,
            Title = article.Title,
            ViewCount = article.ViewCount,
            CommentCount = article.Comments!.Count,
            DatePublished = (DateTime)article.CreatedAt!,
            RatingData = new RatingData
            {
                ThumbsUp = article.ThumbsUp,
                ThumbsDown = article.ThumbsDown,
                UserRating = 0,
            }
        };
    }

    public async Task<List<ArticleCard>> GetLatest(int batchIndex, int batchSize)
    {
        return (await repository.GetLatest(batchIndex, batchSize)).ConvertAll(CreateArticleCard);
    }

    public async Task<List<ArticleCard>> GetLatest(int userId, int batchIndex, int batchSize)
    {
        List<ArticleCard> latest = await GetLatest(batchIndex, batchSize);
        List<int> articleIds = latest.ConvertAll(a => a.ArticleId);
        List<ArticleRating> userRatings = await GetUserRating(userId, articleIds);

        foreach (ArticleCard a in latest)
        {
            ArticleRating? rating = userRatings
                .Where(e => e.ArticleId == a.ArticleId)
                .FirstOrDefault();

            if (rating != null)
            {
                a.RatingData.UserRating = rating.Rate;
            }
        }

        return latest;
    }

    public async Task<ArticlePageModel> View(Article article)
    {
        return new ArticlePageModel
        {
            ArticleId = article.Id,
            Author = new UserLink
            {
                Id = article.Author!.Id,
                Name = article.Author.Name,
                AvatarImage = article.Author.AvatarPath,
            },
            Title = article.Title,
            CategoryCode = article.Category!.Code,
            CategoryName = article.Category.DisplayedName,
            Content = article.Content,
            CoverImage = article.Image,
            Tags = article.Tags!.ConvertAll(t => t.Tag!.Name).ToList(),
            DatePublished = (DateTime)article.CreatedAt!,
            RatingData = new RatingData
            {
                ThumbsUp = article.ThumbsUp,
                ThumbsDown = article.ThumbsDown,
            },
            Views = article.ViewCount,
            Comments = article.Comments!.ConvertAll(commentService.CreateCommentModel)
        };
    }

    public async Task<ArticlePageModel> View(int userId, int articleId)
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

        ArticlePageModel model = await View(article);
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

    public async Task<ArticlePageModel> View(string guestId, int articleId)
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

        ArticlePageModel model = await View(article);

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

    public Task<List<Article>> GetMostPopular()
    {
        return repository.GetMostPopular();
    }

    public async Task<List<TopArticleItem>> GetMostPopularItems()
    {
        return (await GetMostPopular()).ConvertAll(CreateTopArticleItem);
    }
}
