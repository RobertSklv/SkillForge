using SkillForge.Models.Database;

namespace SkillForge.Areas.Admin.Repositories;

public interface IAdminRoleRepository : ICrudRepository<AdminRole>
{
    Task<AdminRole> Get(string code);
}