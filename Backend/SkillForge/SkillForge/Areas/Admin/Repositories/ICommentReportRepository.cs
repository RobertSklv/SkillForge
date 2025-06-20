using SkillForge.Areas.Admin.Models.DTOs;
using SkillForge.Areas.Admin.Models;
using SkillForge.Models.Database;

namespace SkillForge.Areas.Admin.Repositories;

public interface ICommentReportRepository : ICrudRepository<CommentReport>
{
    Task<PaginatedList<CommentReport>> ListClosed(ListingModel listingModel, Func<IQueryable<CommentReport>, IQueryable<CommentReport>>? queryCallback = null);

    Task<int> Close(CommentReport report);

    Task<int> Close(int id);

    Task<int> MassClose(List<int> ids);
}
