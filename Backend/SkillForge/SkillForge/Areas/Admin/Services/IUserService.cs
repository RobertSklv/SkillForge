using Microsoft.AspNetCore.Mvc.ModelBinding;
using SkillForge.Models.Database;
using SkillForge.Models.DTOs.User;

namespace SkillForge.Areas.Admin.Services;

public interface IUserService : ICrudService<User>
{
    Task<User?> GetByName(string name);

    Task<User?> FindUser(string usernameOrEmail);

    Task<bool> IsUsernameTaken(string username);

    Task<bool> IsEmailTaken(string email);

    Task<List<User>> GetMostPopular();

    Task<List<UserLink>> GetMostPopularLinks();

    Task<bool> IsFollowedBy(int id, int followerId);

    Task<bool> IsFollowing(int id, int followedUserId);

    Task<List<UserFollow>> GetFollowings(int id, List<int> followedUserIds);

    Task<List<TagFollow>> GetTagFollowings(int id, List<int> followedTagIds);

    Task<List<UserFollow>> GetLatestFollowers(int userId, int batchIndex, int batchSize);

    Task<List<UserFollow>> GetLatestFollowings(int userId, int batchIndex, int batchSize);

    Task<List<TagFollow>> GetLatestTagFollowings(int userId, int batchIndex, int batchSize);

    Task Follow(int currentUserId, string username);

    Task Unfollow(int currentUserId, string username);

    Task<bool> UpdateInfo(int userId, AccountInfoFormData formData);

    Task<bool> UpdatePassword(int userId, PasswordChangeFormData formData, ModelStateDictionary modelState);

    Task<UserPageData> LoadPage(string name, int? currentUserId = null);
}
