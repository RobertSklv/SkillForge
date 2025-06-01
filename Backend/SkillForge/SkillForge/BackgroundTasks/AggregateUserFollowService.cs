using Microsoft.EntityFrameworkCore;
using SkillForge.Data;
using SkillForge.Models.Database;

namespace SkillForge.BackgroundTasks;

public class AggregateUserFollowService : AggregateService
{
    protected override int FrequencySeconds => 300;

    public AggregateUserFollowService(IServiceScopeFactory scopeFactory) : base(scopeFactory)
    {
    }

    public override async Task Aggregate(AppDbContext db)
    {
        List<UserFollow> allFollows = await db.UserFollows.ToListAsync();
        List<User> users = await db.Users.ToListAsync();

        var followingAggregates = await db.UserFollows
            .GroupBy(e => e.FollowerId)
            .Select(g => new
            {
                UserId = g.Key,
                FollowingCount = g.Count(),
            })
            .ToListAsync();

        var followerAggregates = await db.UserFollows
            .GroupBy(e => e.FollowedUserId)
            .Select(g => new
            {
                UserId = g.Key,
                FollowerCount = g.Count(),
            })
            .ToListAsync();

        foreach (User u in users)
        {
            var followingAggregate = followingAggregates.Where(x => x.UserId == u.Id).FirstOrDefault();
            var followerAggregate = followerAggregates.Where(x => x.UserId == u.Id).FirstOrDefault();

            u.FollowingsCount = followingAggregate?.FollowingCount ?? 0;
            u.FollowersCount = followerAggregate?.FollowerCount ?? 0;
        }

        await db.SaveChangesAsync();
    }
}
