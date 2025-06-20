using SkillForge.Areas.Admin.Models.DTOs;
using SkillForge.Models.Database;
using SkillForge.Models.DTOs.Report;

namespace SkillForge.Areas.Admin.Services;

public interface ICommentReportService : ICrudService<CommentReport>
{
    Task<ListingModel<CommentReport>> CreateClosedReportsListing(ListingModel listingQuery);

    Task Create(int userId, ReportCreateFormData form);

    Task<bool> Close(int id);

    Task<bool> MassClose(List<int> ids);
}
