using Microsoft.EntityFrameworkCore;
using SkillForge.Data;
using SkillForge.Models.Database;

namespace SkillForge.BackgroundTasks;

public class AggregateUserArticlesService : AggregateService
{
    protected override int FrequencySeconds => 300;

    public AggregateUserArticlesService(IServiceScopeFactory scopeFactory) : base(scopeFactory)
    {
    }

    public override async Task Aggregate(AppDbContext db)
    {
        List<Article> allArticles = await db.Articles.ToListAsync();
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
