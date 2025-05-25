using SkillForge.Models.Database;

namespace SkillForge.Areas.Admin.Repositories;

public interface IUserRepository : ICrudRepository<User>
{
    Task<User?> FindUser(string usernameOrEmail);

    Task<bool> IsUsernameTaken(string username);

    Task<bool> IsEmailTaken(string email);
}
