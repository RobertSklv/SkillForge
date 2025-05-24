using SkillForge.Models.Database;

namespace SkillForge.Areas.Admin.Repositories;

public interface IAdminUserRepository
{
    Task<int> SaveUser(AdminUser user);

    Task<AdminUser?> FindUser(string usernameOrEmail);

    Task<bool> IsUsernameTaken(string username);

    Task<bool> IsEmailTaken(string email);

    Task<int> GetAdminUserCount();
}
