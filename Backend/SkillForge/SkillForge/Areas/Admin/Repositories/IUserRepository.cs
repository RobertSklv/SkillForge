﻿using SkillForge.Areas.Admin.Models.DTOs;
using SkillForge.Areas.Admin.Models;
using SkillForge.Models.Database;

namespace SkillForge.Areas.Admin.Repositories;

public interface IUserRepository : ICrudRepository<User>
{
    Task<PaginatedList<User>> ListSuspended(ListingModel listingModel, Func<IQueryable<User>, IQueryable<User>>? queryCallback = null);

    Task<User?> GetByName(string name);

    Task<User?> FindUser(string usernameOrEmail);

    Task<bool> IsUsernameTaken(string username);

    Task<bool> IsEmailTaken(string email);

    Task<List<User>> GetMostPopular();

    Task<bool> IsFollowedBy(int id, int followerId);

    Task<bool> IsFollowing(int id, int followedUserId);

    Task<UserFollow?> GetFollowRecord(int followerId, int followedUserId);

    Task SaveFollowRecord(UserFollow followRecord);

    Task DeleteFollowRecord(UserFollow followRecord);

    Task<List<UserFollow>> GetFollowings(int id, List<int> followedUserIds);

    Task<List<TagFollow>> GetTagFollowings(int id, List<int> followedTagIds);

    Task<List<UserFollow>> GetLatestFollowers(int userId, int batchIndex, int batchSize);

    Task<List<UserFollow>> GetLatestFollowings(int userId, int batchIndex, int batchSize);

    Task<List<TagFollow>> GetLatestTagFollowings(int userId, int batchIndex, int batchSize);

    Task<List<AccountSuspension>> GetSuspensions(int id);

    Task<bool> IsSuspended(int id);

    Task Suspend(int id, Violation reason, byte durationDays, int moderatorId);
}
