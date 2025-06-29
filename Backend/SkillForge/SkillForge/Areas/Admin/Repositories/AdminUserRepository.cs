using Microsoft.EntityFrameworkCore;
using SkillForge.Areas.Admin.Models.Components.Grid;
using SkillForge.Areas.Admin.Models.DTOs;
using SkillForge.Areas.Admin.Models;
using SkillForge.Areas.Admin.Services;
using SkillForge.Data;
using SkillForge.Models.Database;

namespace SkillForge.Areas.Admin.Repositories;

public class AdminUserRepository : CrudRepository<AdminUser>, IAdminUserRepository
{
    public override DbSet<AdminUser> DbSet => db.AdminUsers;

    public AdminUserRepository(
        AppDbContext db,
        IEntityFilterService filterService,
        IEntitySortService sortService,
        IEntitySearchService searchService)
        : base(db, filterService, sortService, searchService)
    {
    }

    public override IQueryable<AdminUser> GetIncludes(IQueryable<AdminUser> query)
    {
        return base.GetIncludes(query)
            .Include(e => e.Role);
    }

    public async Task<int> SaveUser(AdminUser user)
    {
        db.AdminUsers.Add(user);

        return await db.SaveChangesAsync();
    }

    public Task<AdminUser?> FindUser(string usernameOrEmail)
    {
        return db.AdminUsers
            .Include(e => e.Role)
            .FirstOrDefaultAsync(u => u.Name == usernameOrEmail || u.Email == usernameOrEmail);
    }

    public async Task<bool> IsUsernameTaken(string username)
    {
        return await db.AdminUsers.AnyAsync(u => u.Name == username);
    }

    public async Task<bool> IsEmailTaken(string email)
    {
        return await db.AdminUsers.AnyAsync(u => u.Email == email);
    }

    public async Task<int> GetAdminUserCount()
    {
        return await db.AdminUsers.CountAsync();
    }
}
