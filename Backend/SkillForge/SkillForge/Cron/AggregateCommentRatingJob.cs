using Microsoft.EntityFrameworkCore;
using SkillForge.Data;
using SkillForge.Models.Database;

namespace SkillForge.Cron;

public class AggregateCommentRatingJob : IAggregateCommentRatingJob
{
    private readonly AppDbContext db;

    public AggregateCommentRatingJob(AppDbContext db)
    {
        this.db = db;
    }

    public async Task RunAsync()
    {
        List<Comment> comments = await db.Comments.ToListAsync();

        var aggregates = await db.CommentRatings
            .GroupBy(e => e.CommentId)
            .Select(g => new
            {
                CommentId = g.Key,
                ThumbsUpCount = g.Count(r => r.Rate == 1),
                ThumbsDownCount = g.Count(r => r.Rate == -1),
            })
            .ToListAsync();

        foreach (Comment c in comments)
        {
            var aggregate = aggregates.FirstOrDefault(x => x.CommentId == c.Id);

            if (aggregate != null)
            {
                c.ThumbsUp = aggregate.ThumbsUpCount;
                c.ThumbsDown = aggregate.ThumbsDownCount;
            }
            else
            {
                c.ThumbsUp = 0;
                c.ThumbsDown = 0;
            }
        }

        await db.CommentRatings
            .Where(e => e.Rate == 0)
            .ExecuteDeleteAsync();

        await db.SaveChangesAsync();
    }
}
