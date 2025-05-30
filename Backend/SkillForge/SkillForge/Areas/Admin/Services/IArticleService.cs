using SkillForge.Areas.Admin.Models.DTOs;
using SkillForge.Areas.Admin.Models.DTOs.Article;
using SkillForge.Models.Database;

namespace SkillForge.Areas.Admin.Services;

public interface IArticleService : ICrudService<Article>
{
    Task<ListingModel<Article>> CreatePendingArticlesListing(ListingModel listingQuery);

    Task UserCreate(ArticleCreateDTO model, int userId);

    Task<bool> Approve(int id, int adminId);

    Task<List<ArticleCard>> GetLatest(int batchIndex, int batchSize);
}
