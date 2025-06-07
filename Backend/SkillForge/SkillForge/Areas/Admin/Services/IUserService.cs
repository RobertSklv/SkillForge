using SkillForge.Models.Database;
using SkillForge.Models.DTOs.User;

namespace SkillForge.Areas.Admin.Services;

public interface IUserService : ICrudService<User>
{
    Task<User?> FindUser(string usernameOrEmail);

    Task<bool> IsUsernameTaken(string username);

    Task<bool> IsEmailTaken(string email);

    Task<List<User>> GetMostPopular();

    Task<List<UserLink>> GetMostPopularLinks();

    Task<List<UserFollow>> GetFollowings(int id);

    Task<List<UserFollow>> GetFollowers(int id);
}
