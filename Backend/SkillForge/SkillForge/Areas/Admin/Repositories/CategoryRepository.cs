using Microsoft.EntityFrameworkCore;
using SkillForge.Areas.Admin.Services;
using SkillForge.Data;
using SkillForge.Models.Database;

namespace SkillForge.Areas.Admin.Repositories;

public class CategoryRepository : CrudRepository<Category>, ICategoryRepository
{
    public override DbSet<Category> DbSet => db.Categories;

    public CategoryRepository(
        AppDbContext db,
        IEntityFilterService filterService,
        IEntitySortService sortService,
        IEntitySearchService searchService)
        : base(db, filterService, sortService, searchService)
    {
    }

    public async Task<List<Category>> GetPossibleParents(int id)
    {
        if (await DbSet.AnyAsync(e => e.ParentId == id))
        {
            return new();
        }

        return await DbSet
            .Where(e => e.Id != id && e.ParentId == null)
            .ToListAsync();
    }

    public async Task<List<Category>> GetPossibleParents()
    {
        return await DbSet
            .Where(e => e.ParentId == null)
            .ToListAsync();
    }
}
