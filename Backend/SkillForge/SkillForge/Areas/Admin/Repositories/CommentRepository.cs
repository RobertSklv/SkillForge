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
}
