using Microsoft.EntityFrameworkCore;
using SkillForge.Data;
using SkillForge.Models.Database;

namespace SkillForge.Cron;

public class AggregateArticleViewJob : IAggregateArticleViewJob
{
    private readonly AppDbContext db;

    public AggregateArticleViewJob(AppDbContext db)
    {
        this.db = db;
    }

    public async Task RunAsync()
    {
        List<Article> articles = await db.Articles
            .Where(e => e.ApprovalId != null && e.DeleteReason == null)
            .ToListAsync();

        var aggregates = await db.ArticleViews
            .GroupBy(e => e.ArticleId)
            .Select(g => new
            {
                ArticleId = g.Key,
                ViewCount = g.Count()
            })
            .ToListAsync();

        foreach (Article a in articles)
        {
            var aggregate = aggregates.Where(x => x.ArticleId == a.Id).FirstOrDefault();

            a.ViewCount = aggregate?.ViewCount ?? 0;
        }

        await db.SaveChangesAsync();
    }
}
