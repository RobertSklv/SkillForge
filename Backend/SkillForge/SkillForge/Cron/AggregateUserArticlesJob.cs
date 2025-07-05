using Microsoft.EntityFrameworkCore;
using SkillForge.Data;
using SkillForge.Models.Database;

namespace SkillForge.Cron;

public class AggregateUserArticlesJob : IAggregateUserArticlesJob
{
    private readonly AppDbContext db;

    public AggregateUserArticlesJob(AppDbContext db)
    {
        this.db = db;
    }

    public async Task RunAsync()
    {
        List<Article> allArticles = await db.Articles
            .Where(e => e.ApprovalId != null && e.DeleteReason == null)
            .ToListAsync();
        List<User> users = await db.Users.ToListAsync();

        var articleAggregates = await db.Articles
            .Where(e => e.ApprovalId != null)
            .GroupBy(e => e.AuthorId)
            .Select(g => new
            {
                UserId = g.Key,
                ArticleCount = g.Count(),
            })
            .ToListAsync();

        foreach (User u in users)
        {
            var articleAggregate = articleAggregates.Where(x => x.UserId == u.Id).FirstOrDefault();

            u.ArticlesCount = articleAggregate?.ArticleCount ?? 0;
        }

        await db.SaveChangesAsync();
    }
}
