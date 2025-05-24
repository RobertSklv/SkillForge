using Microsoft.EntityFrameworkCore;
using SkillForge.Data;
using SkillForge.Models.Database;

namespace SkillForge.Areas.Admin.Repositories;

public class AdminUserRepository : IAdminUserRepository
{
    private readonly AppDbContext db;

    public AdminUserRepository(AppDbContext db)
    {
        this.db = db;
    }

    public async Task<int> SaveUser(AdminUser user)
    {
        db.AdminUsers.Add(user);

        return await db.SaveChangesAsync();
    }

    public Task<AdminUser?> FindUser(string usernameOrEmail)
    {
        return db.AdminUsers.FirstOrDefaultAsync(u => u.Name == usernameOrEmail || u.Email == usernameOrEmail);
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
