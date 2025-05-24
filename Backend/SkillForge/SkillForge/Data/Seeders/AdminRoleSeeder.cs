using Microsoft.EntityFrameworkCore;
using SkillForge.Models.Database;

namespace SkillForge.Data.Seeders;

public class AdminRoleSeeder : IAdminRoleSeeder
{
    public void Seed(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AdminRole>().HasData(
            new AdminRole
            {
                Id = 1,
                Code = "admin",
                DisplayedName = "Administrator"
            },
            new AdminRole
            {
                Id = 2,
                Code = "mod",
                DisplayedName = "Moderator"
            });
    }
}
