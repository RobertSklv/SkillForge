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

    public Task<List<Tag>> GetMostPopular()
    {
        return DbSet
            .OrderByDescending(e => e.FollowersCount)
            .Take(8)
            .ToListAsync();
    }
}
