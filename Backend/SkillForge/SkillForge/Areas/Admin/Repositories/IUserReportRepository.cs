using SkillForge.Areas.Admin.Models.DTOs;
using SkillForge.Areas.Admin.Models;
using SkillForge.Models.Database;

namespace SkillForge.Areas.Admin.Repositories;

public interface IUserReportRepository : ICrudRepository<UserReport>
{
    Task<PaginatedList<UserReport>> ListClosed(ListingModel listingModel, Func<IQueryable<UserReport>, IQueryable<UserReport>>? queryCallback = null);

    Task<int> Close(UserReport report);

    Task<int> Close(int id);

    Task<int> MassClose(List<int> ids);
}
