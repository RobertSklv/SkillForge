using Microsoft.AspNetCore.Mvc.ModelBinding;
using SkillForge.Areas.Admin.Models;
using SkillForge.Areas.Admin.Models.DTOs;
using SkillForge.Models.Database;
using SkillForge.Models.DTOs.Article;
using SkillForge.Models.DTOs.Rating;
using SkillForge.Models.DTOs.Search;
using SkillForge.Models.DTOs.User;

namespace SkillForge.Areas.Admin.Services;

public interface IArticleService : ICrudService<Article>
{
    Task<bool> SoftDelete(Article article, Violation reason);

    Task<bool> SoftDelete(int id, Violation reason);

    Task SoftDelete(int id, ArticleReport report);

    Task SoftDelete(int id, int articleReportId);

    Task<bool> SoftDeleteMultiple(List<Article> articles, Violation reason);

    Task<bool> SoftDeleteMultiple(List<int> ids, Violation reason);

    Task<bool> Restore(Article article);

    Task<bool> Restore(int id);

    Task<ListingModel<Article>> CreatePendingArticlesListing(ListingModel listingQuery);

    Task<ListingModel<Article>> CreateDeletedArticlesListing(ListingModel listingQuery);

    Task<ListingModel<Article>> CreateListingByTag(ListingModel listingQuery, int tagId);

    Task<PaginatedList<Article>> ListByTag(ListingModel listingModel, int tagId);

    Task UserUpsert(ArticleUpsertFormData model, int userId);

    Task<Article> GetWithComments(int id);

    Task<ArticleRating?> GetUserRating(int userId, int articleId);

    Task<List<ArticleRating>> GetUserRating(int userId, List<int> articleIds);

    Task<List<CommentRating>> GetUserCommentRating(int userId, List<int> commentIds);

    Task<RegisteredArticleView?> GetView(int userId, int articleId);

    Task<GuestArticleView?> GetView(string guestId, int articleId);

    Task RecordView(RegisteredArticleView view);

    Task RecordView(GuestArticleView view);

    void CreateArticleApproval(Article article, int adminId);

    Task<bool> Approve(int id, int adminId);

    Task<bool> MassApprove(List<int> ids, int adminId);

    bool ValidateTagNames(List<string> tags, string propertyName, ModelStateDictionary modelState);

    Task<ArticlePageData> View(int userId, int articleId);

    Task<ArticlePageData> View(string? guestId, int articleId);

    Task<ArticleUpsertPageModel> LoadUpsertPage(int? id, int userId);

    Task Rate(int userId, int articleId, UserRatingData rate);

    Task<List<Article>> Search(string phrase);

    Task<List<ArticleSearchItem>> SearchItems(string phrase);

    Task<PaginationResponse<ArticleCard>> SearchAdvancedCards(GridState gridState);

    Task<List<UserListItem>> GetRating(int articleId, int? currentUserId, int batchIndex, int batchSize, bool positive);
}
