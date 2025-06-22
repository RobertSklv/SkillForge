using Microsoft.EntityFrameworkCore;
using SkillForge.Areas.Admin.Models.DTOs;
using SkillForge.Areas.Admin.Models;
using SkillForge.Areas.Admin.Services;
using SkillForge.Data;
using SkillForge.Models.Database;

namespace SkillForge.Areas.Admin.Repositories;

public class UserReportRepository : CrudRepository<UserReport>, IUserReportRepository
{
    public override DbSet<UserReport> DbSet => db.UserReports;

    public UserReportRepository(
        AppDbContext db,
        IEntityFilterService filterService,
        IEntitySortService sortService,
        IEntitySearchService searchService)
        : base(db, filterService, sortService, searchService)
    {
    }

    public override IQueryable<UserReport> GetIncludes(IQueryable<UserReport> query)
    {
        return base.GetIncludes(query)
            .Include(e => e.Reporter)
            .Include(e => e.ReportedUser);
    }

    public override Task<PaginatedList<UserReport>> List(ListingModel listingModel, Func<IQueryable<UserReport>, IQueryable<UserReport>>? queryCallback = null)
    {
        return base.List(listingModel, query =>
        {
            // List only open user reports by default.
            query = query.Where(e => !e.IsClosed);

            if (queryCallback != null)
            {
                query = queryCallback.Invoke(query);
            }

            return query;
        });
    }

    public Task<PaginatedList<UserReport>> ListClosed(ListingModel listingModel, Func<IQueryable<UserReport>, IQueryable<UserReport>>? queryCallback = null)
    {
        return base.List(listingModel, query =>
        {
            query = query.Where(e => e.IsClosed);

            if (queryCallback != null)
            {
                query = queryCallback.Invoke(query);
            }

            return query;
        });
    }

    public Task<int> Close(UserReport report)
    {
        report.IsClosed = true;

        return db.SaveChangesAsync();
    }

    public async Task<int> Close(int id)
    {
        UserReport report = await GetStrict(id);

        return await Close(report);
    }

    public async Task<int> MassClose(List<int> ids)
    {
        List<UserReport> reports = await GetByIds(ids);

        reports.ForEach(r => r.IsClosed = true);

        return await db.SaveChangesAsync();
    }
}
