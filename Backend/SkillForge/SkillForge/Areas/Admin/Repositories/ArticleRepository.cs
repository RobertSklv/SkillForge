using Microsoft.EntityFrameworkCore;
using SkillForge.Areas.Admin.Models;
using SkillForge.Areas.Admin.Models.DTOs;
using SkillForge.Areas.Admin.Services;
using SkillForge.Data;
using SkillForge.Models.Database;

namespace SkillForge.Areas.Admin.Repositories;

public class ArticleRepository : CrudRepository<Article>, IArticleRepository
{
    public override DbSet<Article> DbSet => db.Articles;

    public ArticleRepository(
        AppDbContext db,
        IEntityFilterService filterService,
        IEntitySortService sortService,
        IEntitySearchService searchService)
        : base(db, filterService, sortService, searchService)
    {
    }

    public override IQueryable<Article> GetIncludes(IQueryable<Article> query)
    {
        return base.GetIncludes(query)
            .Include(e => e.Author)
            .Include(e => e.Category);
    }

    public override Task<PaginatedList<Article>> List(ListingModel listingModel, Func<IQueryable<Article>, IQueryable<Article>>? queryCallback = null)
    {
        return base.List(listingModel, query =>
        {
            // List only approved articles by default.
            query = query.Where(e => e.ApprovalId != null);

            if (queryCallback != null)
            {
                query = queryCallback.Invoke(query);
            }

            return query;
        });
    }

    public Task<PaginatedList<Article>> ListPending(ListingModel listingModel, Func<IQueryable<Article>, IQueryable<Article>>? queryCallback = null)
    {
        return base.List(listingModel, query =>
        {
            query = query.Where(e => e.ApprovalId == null);

            if (queryCallback != null)
            {
                query = queryCallback.Invoke(query);
            }

            return query;
        });
    }
}
