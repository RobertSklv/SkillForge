using Microsoft.EntityFrameworkCore;
using SkillForge.Areas.Admin.Services;
using SkillForge.Data;
using SkillForge.Models.Database;

namespace SkillForge.Areas.Admin.Repositories;

public class AdminRoleRepository : CrudRepository<AdminRole>, IAdminRoleRepository
{
    public override DbSet<AdminRole> DbSet => db.AdminRoles;

    public AdminRoleRepository(
        AppDbContext db,
        IEntityFilterService filterService,
        IEntitySortService sortService,
        IEntitySearchService searchService)
        : base(db, filterService, sortService, searchService)
    {
    }

    public async Task<AdminRole> Get(string code)
    {
        return await db.AdminRoles.FirstOrDefaultAsync(e => e.Code == code) ?? throw new Exception($"Admin role not found: {code}");
    }
}
