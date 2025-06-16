using Ganss.Xss;
using Microsoft.EntityFrameworkCore;
using SkillForge.Areas.Admin.Services;
using SkillForge.Data;
using SkillForge.Models.Database;

namespace SkillForge.Areas.Admin.Repositories;

public class CommentRepository : CrudRepository<Comment>, ICommentRepository
{
    public override DbSet<Comment> DbSet => db.Comments;

    public CommentRepository(
        AppDbContext db,
        IEntityFilterService filterService,
        IEntitySortService sortService,
        IEntitySearchService searchService)
        : base(db, filterService, sortService, searchService)
    {
    }

    public override Task<int> Upsert(Comment entity)
    {
        HtmlSanitizer sanitizer = new();

        entity.Content = sanitizer.Sanitize(entity.Content);

        entity.Content = entity.Content.Replace("<a", "<a rel=\"nofollow ugc\"");

        return base.Upsert(entity);
    }

    public Task<CommentRating?> GetUserRating(int userId, int commentId)
    {
        return db.CommentRatings.FirstOrDefaultAsync(e => e.UserId == userId && e.CommentId == commentId);
    }

    public Task UpsertUserRating(CommentRating rating)
    {
        if (rating.Id == 0)
        {
            db.CommentRatings.Add(rating);
        }
        else
        {
            db.CommentRatings.Update(rating);
        }

        return db.SaveChangesAsync();
    }

    public Task<List<CommentRating>> GetRating(int commentId, int batchIndex, int batchSize, bool positive)
    {
        int rate = positive ? 1 : -1;

        return db.CommentRatings
            .Include(e => e.User)
            .Where(e => e.CommentId == commentId && e.Rate == rate)
            .OrderByDescending(e => e.CreatedAt)
            .Skip(batchIndex * batchSize)
            .Take(batchSize)
            .ToListAsync();
    }
}
