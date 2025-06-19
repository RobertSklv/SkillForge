using Microsoft.EntityFrameworkCore;
using SkillForge.Areas.Admin.Models.DTOs;
using SkillForge.Areas.Admin.Models;
using SkillForge.Areas.Admin.Services;
using SkillForge.Data;
using SkillForge.Models.Database;

namespace SkillForge.Areas.Admin.Repositories;

public class ArticleReportRepository : CrudRepository<ArticleReport>, IArticleReportRepository
{
    public override DbSet<ArticleReport> DbSet => db.ArticleReports;

    public ArticleReportRepository(
        AppDbContext db,
        IEntityFilterService filterService,
        IEntitySortService sortService,
        IEntitySearchService searchService)
        : base(db, filterService, sortService, searchService)
    {
    }

    public override IQueryable<ArticleReport> GetIncludes(IQueryable<ArticleReport> query)
    {
        return base.GetIncludes(query)
            .Include(e => e.Reporter)
            .Include(e => e.Article);
    }

    public override Task<PaginatedList<ArticleReport>> List(ListingModel listingModel, Func<IQueryable<ArticleReport>, IQueryable<ArticleReport>>? queryCallback = null)
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

    public Task<PaginatedList<ArticleReport>> ListClosed(ListingModel listingModel, Func<IQueryable<ArticleReport>, IQueryable<ArticleReport>>? queryCallback = null)
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

    public Task<int> Close(ArticleReport report)
    {
        report.IsClosed = true;

        return db.SaveChangesAsync();
    }

    public async Task<int> Close(int id)
    {
        ArticleReport report = await GetStrict(id);

        return await Close(report);
    }

    public async Task<int> MassClose(List<int> ids)
    {
        List<ArticleReport> reports = await GetByIds(ids);

        reports.ForEach(r => r.IsClosed = true);

        return await db.SaveChangesAsync();
    }
}
