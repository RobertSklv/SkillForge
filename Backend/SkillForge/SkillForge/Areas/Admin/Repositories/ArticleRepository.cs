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

    public async Task<Article> GetWithComments(int id)
    {
        return await GetIncludes(DbSet)
            .Include(e => e.Comments!)
                .ThenInclude(e => e.User)
            .FirstOrDefaultAsync(e => e.Id == id)
            ?? throw new Exception($"No article with Id {id} exists.");
    }

    public Task<ArticleRating?> GetUserRating(int userId, int articleId)
    {
        return db.ArticleRatings.FirstOrDefaultAsync(e => e.UserId == userId && e.ArticleId == articleId);
    }

    public Task UpsertUserRating(ArticleRating rating)
    {
        if (rating.Id == 0)
        {
            db.ArticleRatings.Add(rating);
        }
        else
        {
            db.ArticleRatings.Update(rating);
        }

        return db.SaveChangesAsync();
    }

    public Task<RegisteredArticleView?> GetView(int userId, int articleId)
    {
        return db.RegisteredArticleViews.FirstOrDefaultAsync(e => e.UserId == userId && e.ArticleId == articleId);
    }

    public Task<GuestArticleView?> GetView(string guestId, int articleId)
    {
        return db.GuestArticleViews.FirstOrDefaultAsync(e => e.GuestId == guestId && e.ArticleId == articleId);
    }

    public Task RecordView(RegisteredArticleView view)
    {
        db.RegisteredArticleViews.Add(view);

        return db.SaveChangesAsync();
    }

    public Task RecordView(GuestArticleView view)
    {
        db.GuestArticleViews.Add(view);

        return db.SaveChangesAsync();
    }

    public override IQueryable<Article> GetIncludes(IQueryable<Article> query)
    {
        return base.GetIncludes(query)
            .Include(e => e.Author)
            .Include(e => e.Category)
            .Include(e => e.Tags!)
                .ThenInclude(e => e.Tag);
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

    public Task<PaginatedList<Article>> ListByTag(ListingModel listingModel, int tagId)
    {
        return base.List(listingModel, query =>
        {
            return query.Where(e => e.Tags!.Any(e => e.TagId == tagId));
        });
    }

    public Task<List<Article>> GetLatest(int batchIndex, int batchSize)
    {
        return DbSet
            .Include(e => e.Author)
            .Include(e => e.Category)
            .Include(e => e.Approval)
            .Where(e => e.ApprovalId != null)
            .OrderByDescending(e => e.CreatedAt)
            .Skip(batchIndex * batchSize)
            .Take(batchSize)
            .ToListAsync();
    }
}
