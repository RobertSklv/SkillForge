using Microsoft.EntityFrameworkCore;
using SkillForge.Areas.Admin.Services;
using SkillForge.Data;
using SkillForge.Models.Database;

namespace SkillForge.Areas.Admin.Repositories;

public class GuestArticleViewRepository : CrudRepository<GuestArticleView>, IGuestArticleViewRepository
{
    public override DbSet<GuestArticleView> DbSet => db.GuestArticleViews;

    public GuestArticleViewRepository(
        AppDbContext db,
        IEntityFilterService filterService,
        IEntitySortService sortService,
        IEntitySearchService searchService)
        : base(db, filterService, sortService, searchService)
    {
    }
}
