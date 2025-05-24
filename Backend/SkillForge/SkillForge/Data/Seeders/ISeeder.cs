using Microsoft.EntityFrameworkCore;

namespace SkillForge.Data.Seeders;

public interface ISeeder
{
    void Seed(ModelBuilder modelBuilder);
}
