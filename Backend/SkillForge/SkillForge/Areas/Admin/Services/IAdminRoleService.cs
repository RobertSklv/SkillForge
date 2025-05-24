using SkillForge.Models.Database;

namespace SkillForge.Areas.Admin.Services;

public interface IAdminRoleService
{
    Task<AdminRole> Get(string code);
}
