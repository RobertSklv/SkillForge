using SkillForge.Models.Database;

namespace SkillForge.Areas.Admin.Repositories;

public interface IAdminRoleRepository
{
    Task<AdminRole> Get(string code);
}