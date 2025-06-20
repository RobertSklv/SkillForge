using Microsoft.EntityFrameworkCore;
using SkillForge.Areas.Admin.Models.DTOs;
using SkillForge.Areas.Admin.Models;
using SkillForge.Areas.Admin.Services;
using SkillForge.Data;
using SkillForge.Models.Database;

namespace SkillForge.Areas.Admin.Repositories;

public class CommentReportRepository : CrudRepository<CommentReport>, ICommentReportRepository
{
    public override DbSet<CommentReport> DbSet => db.CommentReports;

    public CommentReportRepository(
        AppDbContext db,
        IEntityFilterService filterService,
        IEntitySortService sortService,
        IEntitySearchService searchService)
        : base(db, filterService, sortService, searchService)
    {
    }

    public override IQueryable<CommentReport> GetIncludes(IQueryable<CommentReport> query)
    {
        return base.GetIncludes(query)
            .Include(e => e.Reporter)
            .Include(e => e.Comment);
    }

    public override Task<PaginatedList<CommentReport>> List(ListingModel listingModel, Func<IQueryable<CommentReport>, IQueryable<CommentReport>>? queryCallback = null)
    {
        return base.List(listingModel, query =>
        {
            // List only open article reports by default.
            query = query.Where(e => !e.IsClosed);

            if (queryCallback != null)
            {
                query = queryCallback.Invoke(query);
            }

            return query;
        });
    }

    public Task<PaginatedList<CommentReport>> ListClosed(ListingModel listingModel, Func<IQueryable<CommentReport>, IQueryable<CommentReport>>? queryCallback = null)
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

    public Task<int> Close(CommentReport report)
    {
        report.IsClosed = true;

        return db.SaveChangesAsync();
    }

    public async Task<int> Close(int id)
    {
        CommentReport report = await GetStrict(id);

        return await Close(report);
    }

    public async Task<int> MassClose(List<int> ids)
    {
        List<CommentReport> reports = await GetByIds(ids);

        reports.ForEach(r => r.IsClosed = true);

        return await db.SaveChangesAsync();
    }
}
