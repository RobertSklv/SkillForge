using SkillForge.Areas.Admin.Models.DTOs;
using SkillForge.Areas.Admin.Models;
using SkillForge.Models.Database;

namespace SkillForge.Areas.Admin.Repositories;

public interface IArticleReportRepository : ICrudRepository<ArticleReport>
{
    Task<PaginatedList<ArticleReport>> ListClosed(ListingModel listingModel, Func<IQueryable<ArticleReport>, IQueryable<ArticleReport>>? queryCallback = null);

    Task<int> Close(ArticleReport report);

    Task<int> Close(int id);

    Task<int> MassClose(List<int> ids);
}
