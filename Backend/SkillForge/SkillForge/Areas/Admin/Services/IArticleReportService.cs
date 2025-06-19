using SkillForge.Areas.Admin.Models.DTOs;
using SkillForge.Models.Database;
using SkillForge.Models.DTOs.ArticleReport;

namespace SkillForge.Areas.Admin.Services;

public interface IArticleReportService : ICrudService<ArticleReport>
{
    Task<ListingModel<ArticleReport>> CreateClosedReportsListing(ListingModel listingQuery);

    ArticleReportCreateFormOptions GetFormOptions();

    string GetViolationTitle(Violation v);

    Task Create(int userId, ArticleReportCreate form);

    Task<bool> Close(int id);

    Task<bool> MassClose(List<int> ids);
}
