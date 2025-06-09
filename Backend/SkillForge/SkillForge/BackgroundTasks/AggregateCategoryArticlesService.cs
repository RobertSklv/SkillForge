using Microsoft.EntityFrameworkCore;
using SkillForge.Data;
using SkillForge.Models.Database;

namespace SkillForge.BackgroundTasks;

public class AggregateCategoryArticlesService : AggregateService
{
    protected override int FrequencySeconds => 300;

    public AggregateCategoryArticlesService(IServiceScopeFactory scopeFactory) : base(scopeFactory)
    {
    }

    public override async Task Aggregate(AppDbContext db)
    {
        List<Article> allArticles = await db.Articles.ToListAsync();
        List<Category> categories = await db.Categories.ToListAsync();

        var articleAggregates = await db.Articles
            .Where(e => e.ApprovalId != null)
            .GroupBy(e => e.CategoryId)
            .Select(g => new
            {
                CategoryId = g.Key,
                ArticleCount = g.Count(),
            })
            .ToListAsync();

        foreach (Category c in categories)
        {
            var articleAggregate = articleAggregates.Where(x => x.CategoryId == c.Id).FirstOrDefault();

            c.ArticlesCount = articleAggregate?.ArticleCount ?? 0;
        }

        await db.SaveChangesAsync();
    }
}
