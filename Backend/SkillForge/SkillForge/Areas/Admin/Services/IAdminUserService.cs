using SkillForge.Areas.Admin.Models.DTOs;
using SkillForge.Models.Database;

namespace SkillForge.Areas.Admin.Services;

public interface IAdminUserService : ICrudService<AdminUser, AdminUserVM>
{
    Task<bool> Upsert(AdminUserVM model, bool isAdministrator);
}
