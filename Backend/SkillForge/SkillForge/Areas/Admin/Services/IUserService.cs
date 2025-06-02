using SkillForge.Areas.Admin.Models.DTOs;
using SkillForge.Models.Database;
using SkillForge.Models.DTOs;

namespace SkillForge.Areas.Admin.Services;

public interface IUserService : ICrudService<User>
{
    Task<User?> FindUser(string usernameOrEmail);

    Task<bool> IsUsernameTaken(string username);

    Task<bool> IsEmailTaken(string email);

    UserInfo GetUserInfo(User user);

    Task<List<User>> GetMostPopular();

    Task<List<UserLink>> GetMostPopularLinks();
}
