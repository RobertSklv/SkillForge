using Microsoft.EntityFrameworkCore;
using SkillForge.Data;
using SkillForge.Models.Database;

namespace SkillForge.Cron;

public class AggregateTagArticleJob : IAggregateTagArticleJob
{
    private readonly AppDbContext db;

    public AggregateTagArticleJob(AppDbContext db)
    {
        this.db = db;
    }

    public async Task RunAsync()
    {
        List<ArticleTag> allTags = await db.ArticleTags.ToListAsync();
        List<Tag> tags = await db.Tags.ToListAsync();

        var articleAggregates = await db.ArticleTags
            .GroupBy(e => e.TagId)
            .Select(g => new
            {
                TagId = g.Key,
                ArticleCount = g.Count(),
            })
            .ToListAsync();

        foreach (Tag t in tags)
        {
            var articleAggregate = articleAggregates.Where(x => x.TagId == t.Id).FirstOrDefault();

            t.ArticlesCount = articleAggregate?.ArticleCount ?? 0;
        }

        await db.SaveChangesAsync();
    }
}
