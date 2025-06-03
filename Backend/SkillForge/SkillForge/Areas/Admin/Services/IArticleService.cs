using Microsoft.AspNetCore.Mvc.ModelBinding;
using SkillForge.Areas.Admin.Models;
using SkillForge.Areas.Admin.Models.DTOs;
using SkillForge.Areas.Admin.Models.DTOs.Article;
using SkillForge.Areas.Admin.Models.DTOs.Rating;
using SkillForge.Models.Database;

namespace SkillForge.Areas.Admin.Services;

public interface IArticleService : ICrudService<Article>
{
    Task<ListingModel<Article>> CreatePendingArticlesListing(ListingModel listingQuery);

    Task<ListingModel<Article>> CreateListingByTag(ListingModel listingQuery, int tagId);

    Task<PaginatedList<Article>> ListByTag(ListingModel listingModel, int tagId);

    Task UserUpsert(ArticleUpsertDTO model, int userId);

    Task<Article> GetWithComments(int id);

    Task<ArticleRating?> GetUserRating(int userId, int articleId);

    Task<List<ArticleRating>> GetUserRating(int userId, List<int> articleIds);

    Task<List<CommentRating>> GetUserCommentRating(int userId, List<int> commentIds);

    Task<RegisteredArticleView?> GetView(int userId, int articleId);

    Task<GuestArticleView?> GetView(string guestId, int articleId);

    Task RecordView(RegisteredArticleView view);

    Task RecordView(GuestArticleView view);

    Task<bool> Approve(int id, int adminId);

    Task<bool> MassApprove(List<int> ids, int adminId);

    bool ValidateTagNames(List<string> tags, string propertyName, ModelStateDictionary modelState);

    ArticleCard CreateArticleCard(Article article);

    TopArticleItem CreateTopArticleItem(Article article);

    Task<List<ArticleCard>> GetLatest(int batchIndex, int batchSize);

    Task<List<ArticleCard>> GetLatest(int userId, int batchIndex, int batchSize);

    Task<ArticlePageModel> View(int userId, int articleId);

    Task<ArticlePageModel> View(string guestId, int articleId);

    Task Rate(int userId, int articleId, UserRatingData rate);

    Task<List<Article>> GetMostPopular();

    Task<List<TopArticleItem>> GetMostPopularItems();
}
