using Microsoft.EntityFrameworkCore;
using SkillForge.Areas.Admin.Models.DTOs;
using SkillForge.Areas.Admin.Models;
using SkillForge.Areas.Admin.Services;
using SkillForge.Data;
using SkillForge.Models.Database;

namespace SkillForge.Areas.Admin.Repositories;

public class UserRepository : CrudRepository<User>, IUserRepository
{
    public override DbSet<User> DbSet => db.Users;

    public UserRepository(
        AppDbContext db,
        IEntityFilterService filterService,
        IEntitySortService sortService,
        IEntitySearchService searchService)
        : base(db, filterService, sortService, searchService)
    {
    }

    public override Task<PaginatedList<User>> List(ListingModel listingModel, Func<IQueryable<User>, IQueryable<User>>? queryCallback = null)
    {
        return base.List(listingModel, query =>
        {
            query = query
                .Include(e => e.Suspensions)
                .Where(e => !e.Suspensions!.Any(e => DateTime.Now < e.CreatedAt!.Value.AddDays(e.DurationDays)));

            if (queryCallback != null)
            {
                query = queryCallback.Invoke(query);
            }

            return query;
        });
    }

    public Task<PaginatedList<User>> ListSuspended(ListingModel listingModel, Func<IQueryable<User>, IQueryable<User>>? queryCallback = null)
    {
        return base.List(listingModel, query =>
        {
            query = query
                .Include(e => e.Suspensions)
                .Where(e => e.Suspensions!.Any(e => DateTime.Now < e.CreatedAt!.Value.AddDays(e.DurationDays)));

            if (queryCallback != null)
            {
                query = queryCallback.Invoke(query);
            }

            return query;
        });
    }

    public Task<User?> GetByName(string name)
    {
        return DbSet.FirstOrDefaultAsync(e => e.Name == name);
    }

    public Task<User?> FindUser(string usernameOrEmail)
    {
        return DbSet.FirstOrDefaultAsync(u => u.Name == usernameOrEmail || u.Email == usernameOrEmail);
    }

    public async Task<bool> IsUsernameTaken(string username)
    {
        return await DbSet.AnyAsync(u => u.Name == username);
    }

    public async Task<bool> IsEmailTaken(string email)
    {
        return await DbSet.AnyAsync(u => u.Email == email);
    }

    public Task<List<User>> GetMostPopular()
    {
        return DbSet
            .OrderByDescending(e => e.FollowersCount)
            .Take(8)
            .ToListAsync();
    }

    public Task<bool> IsFollowedBy(int id, int followerId)
    {
        return db.UserFollows.AnyAsync(e => e.FollowedUserId == id && e.FollowerId == followerId);
    }

    public Task<bool> IsFollowing(int id, int followedUserId)
    {
        return db.UserFollows.AnyAsync(e => e.FollowerId == id && e.FollowedUserId == followedUserId);
    }

    public Task<UserFollow?> GetFollowRecord(int followerId, int followedUserId)
    {
        return db.UserFollows.FirstOrDefaultAsync(e => e.FollowerId == followerId && e.FollowedUserId == followedUserId);
    }

    public Task SaveFollowRecord(UserFollow followRecord)
    {
        db.UserFollows.Add(followRecord);

        return db.SaveChangesAsync();
    }

    public Task DeleteFollowRecord(UserFollow followRecord)
    {
        db.UserFollows.Remove(followRecord);

        return db.SaveChangesAsync();
    }

    public Task<List<UserFollow>> GetFollowings(int id, List<int> followedUserIds)
    {
        return db.UserFollows
            .Where(e => e.FollowerId == id && followedUserIds.Contains(e.FollowedUserId))
            .ToListAsync();
    }

    public Task<List<TagFollow>> GetTagFollowings(int id, List<int> followedTagIds)
    {
        return db.TagFollows
            .Where(e => e.UserId == id && followedTagIds.Contains(e.TagId))
            .ToListAsync();
    }

    public Task<List<UserFollow>> GetLatestFollowers(int userId, int batchIndex, int batchSize)
    {
        return db.UserFollows
            .Include(e => e.Follower)
            .Where(e => e.FollowedUserId == userId)
            .OrderByDescending(e => e.CreatedAt)
            .Skip(batchIndex * batchSize)
            .Take(batchSize)
            .ToListAsync();
    }

    public Task<List<UserFollow>> GetLatestFollowings(int userId, int batchIndex, int batchSize)
    {
        return db.UserFollows
            .Include(e => e.FollowedUser)
            .Where(e => e.FollowerId == userId)
            .OrderByDescending(e => e.CreatedAt)
            .Skip(batchIndex * batchSize)
            .Take(batchSize)
            .ToListAsync();
    }

    public Task<List<TagFollow>> GetLatestTagFollowings(int userId, int batchIndex, int batchSize)
    {
        return db.TagFollows
            .Include(e => e.Tag)
            .Where(e => e.UserId == userId)
            .OrderByDescending(e => e.CreatedAt)
            .Skip(batchIndex * batchSize)
            .Take(batchSize)
            .ToListAsync();
    }

    public Task<List<AccountSuspension>> GetSuspensions(int id)
    {
        return db.AccountSuspensions
            .Include(e => e.Moderator)
            .Where(e => e.UserId == id)
            .ToListAsync();
    }

    public Task<bool> IsSuspended(int id)
    {
        return db.AccountSuspensions
            .AnyAsync(e =>
                e.UserId == id
                &&
                DateTime.Now < e.CreatedAt!.Value.AddDays(e.DurationDays));
    }

    public async Task Suspend(int id, Violation reason, byte durationDays, int moderatorId)
    {
        AccountSuspension suspension = new()
        {
            UserId = id,
            DurationDays = durationDays,
            ModeratorId = moderatorId,
            Reason = reason,
        };

        db.AccountSuspensions.Add(suspension);

        await db.SaveChangesAsync();
    }
}
