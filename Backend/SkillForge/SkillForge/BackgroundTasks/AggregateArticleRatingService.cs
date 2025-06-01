using Microsoft.EntityFrameworkCore;
using SkillForge.Data;
using SkillForge.Models.Database;

namespace SkillForge.BackgroundTasks;

public class AggregateArticleRatingService : AggregateService
{
    protected override int FrequencySeconds => 60;

    public AggregateArticleRatingService(IServiceScopeFactory scopeFactory)
        : base(scopeFactory)
    {
    }

    public override async Task Aggregate(AppDbContext db)
    {
        List<Article> articles = await db.Articles.ToListAsync();

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
