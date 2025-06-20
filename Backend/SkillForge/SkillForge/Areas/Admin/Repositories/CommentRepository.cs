using Ganss.Xss;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using SkillForge.Areas.Admin.Services;
using SkillForge.Data;
using SkillForge.Models.Database;

namespace SkillForge.Areas.Admin.Repositories;

public class CommentRepository : CrudRepository<Comment>, ICommentRepository
{
    private readonly ICommentReportRepository commentReportRepository;

    public override DbSet<Comment> DbSet => db.Comments;

    public CommentRepository(
        AppDbContext db,
        IEntityFilterService filterService,
        IEntitySortService sortService,
        IEntitySearchService searchService,
        ICommentReportRepository commentReportRepository)
        : base(db, filterService, sortService, searchService)
    {
        this.commentReportRepository = commentReportRepository;
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

    public override Task<int> Delete(int id)
    {
        throw new InvalidOperationException($"Invalid delete method. Use {nameof(SoftDelete)} instead.");
    }

    public override Task<int> DeleteMultiple(List<int> ids)
    {
        throw new InvalidOperationException($"Invalid delete method. Use {nameof(SoftDelete)} instead.");
    }

    public Task<int> SoftDelete(Comment comment, Violation reason)
    {
        comment.DeleteReason = reason;

        return db.SaveChangesAsync();
    }

    public async Task<int> SoftDelete(int id, Violation reason)
    {
        Comment comment = await GetStrict(id);

        return await SoftDelete(comment, reason);
    }

    public async Task SoftDelete(int id, CommentReport report)
    {
        using IDbContextTransaction transaction = await db.Database.BeginTransactionAsync();

        try
        {
            await SoftDelete(id, report.Reason);
            await commentReportRepository.Close(report);

            await transaction.CommitAsync();
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();

            throw;
        }
    }

    public async Task<int> SoftDeleteMultiple(List<Comment> comments, Violation reason)
    {
        comments.ForEach(comment => comment.DeleteReason = reason);

        return await db.SaveChangesAsync();
    }

    public async Task<int> SoftDeleteMultiple(List<int> ids, Violation reason)
    {
        List<Comment> comments = await GetByIds(ids);

        return await SoftDeleteMultiple(comments, reason);
    }

    public Task<int> Restore(Comment comment)
    {
        comment.DeleteReason = null;

        return db.SaveChangesAsync();
    }

    public async Task<int> Restore(int id)
    {
        Comment comment = await GetStrict(id);

        return await Restore(comment);
    }
}
