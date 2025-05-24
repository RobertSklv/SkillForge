using Microsoft.EntityFrameworkCore;
using SkillForge.Data;
using SkillForge.Models.Database;

namespace SkillForge.Areas.Admin.Repositories;

public class AdminRoleRepository : IAdminRoleRepository
{
    private readonly AppDbContext db;

    public AdminRoleRepository(AppDbContext db)
	{
        this.db = db;
    }

    public async Task<AdminRole> Get(string code)
    {
        return await db.AdminRoles.FirstOrDefaultAsync(e => e.Code == code) ?? throw new Exception($"Admin role not found: {code}");
    }
}
