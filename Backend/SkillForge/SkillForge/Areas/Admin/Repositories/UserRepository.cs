using Microsoft.EntityFrameworkCore;
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
    
    public Task<List<UserFollow>> GetFollowings(int id)
    {
        return db.UserFollows
            .Where(e => e.FollowerId == id)
            .ToListAsync();
    }
    
    public Task<List<UserFollow>> GetFollowers(int id)
    {
        return db.UserFollows
            .Where(e => e.FollowedUserId == id)
            .ToListAsync();
    }
}
