using SkillForge.Models.Database;

namespace SkillForge.Areas.Admin.Services;

public interface IAdminRoleService : ICrudService<AdminRole>
{
    Task<AdminRole> Get(string code);
}
