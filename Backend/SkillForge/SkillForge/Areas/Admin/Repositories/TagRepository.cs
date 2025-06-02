using Microsoft.EntityFrameworkCore;
using SkillForge.Areas.Admin.Services;
using SkillForge.Data;
using SkillForge.Models.Database;

namespace SkillForge.Areas.Admin.Repositories;

public class TagRepository : CrudRepository<Tag>, ITagRepository
{
    public override DbSet<Tag> DbSet => db.Tags;

    public TagRepository(
        AppDbContext db,
        IEntityFilterService filterService,
        IEntitySortService sortService,
        IEntitySearchService searchService)
        : base(db, filterService, sortService, searchService)
    {
    }

    public Task<Tag?> GetByName(string name)
    {
        return DbSet.FirstOrDefaultAsync(e => e.Name == name);
    }

    public Task<List<Tag>> GetByNames(List<string> names)
    {
        return DbSet
            .Where(e => names.Contains(e.Name))
            .ToListAsync();
    }

    public Task<List<Tag>> GetMostPopular()
    {
        return DbSet
            .OrderByDescending(e => e.FollowersCount)
            .Take(8)
            .ToListAsync();
    }

    public Task<List<Tag>> Search(string? phrase, List<string>? exclude)
    {
        IQueryable<Tag> query = DbSet;

        if (!string.IsNullOrEmpty(phrase))
        {
            query = query.Where(e => e.Name.Contains(phrase) || e.Name.StartsWith(phrase) || e.Name.EndsWith(phrase));
        }

        if (exclude != null && exclude.Count > 0)
        {
            query = query.Where(e => !exclude.Contains(e.Name));
        }

        return query
            .OrderByDescending(e => e.ArticlesCount)
            .Take(6)
            .ToListAsync();
    }
}
