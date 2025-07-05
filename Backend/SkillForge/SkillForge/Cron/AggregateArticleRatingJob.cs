using Microsoft.EntityFrameworkCore;
using SkillForge.Data;
using SkillForge.Models.Database;

namespace SkillForge.Cron;

public class AggregateArticleRatingJob : IAggregateArticleRatingJob
{
    private readonly AppDbContext db;

    public AggregateArticleRatingJob(AppDbContext db)
    {
        this.db = db;
    }

    public async Task RunAsync()
    {
        List<Article> articles = await db.Articles
            .Where(e => e.ApprovalId != null && e.DeleteReason == null)
            .ToListAsync();

        var aggregates = await db.ArticleRatings
            .GroupBy(e => e.ArticleId)
            .Select(g => new
            {
                ArticleId = g.Key,
                ThumbsUpCount = g.Count(r => r.Rate == 1),
                ThumbsDownCount = g.Count(r => r.Rate == -1),
            })
            .ToListAsync();

        foreach (Article a in articles)
        {
            var aggregate = aggregates.FirstOrDefault(x => x.ArticleId == a.Id);

            if (aggregate != null)
            {
                a.ThumbsUp = aggregate.ThumbsUpCount;
                a.ThumbsDown = aggregate.ThumbsDownCount;
            }
            else
            {
                a.ThumbsUp = 0;
                a.ThumbsDown = 0;
            }
        }

        await db.ArticleRatings
            .Where(e => e.Rate == 0)
            .ExecuteDeleteAsync();

        await db.SaveChangesAsync();
    }
}
