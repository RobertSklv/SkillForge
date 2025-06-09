using Microsoft.EntityFrameworkCore;
using SkillForge.Data;
using SkillForge.Models.Database;

namespace SkillForge.BackgroundTasks;

public class AggregateTagFollowService : AggregateService
{
    protected override int FrequencySeconds => 300;

    public AggregateTagFollowService(IServiceScopeFactory scopeFactory) : base(scopeFactory)
    {
    }

    public override async Task Aggregate(AppDbContext db)
    {
        List<TagFollow> allFollows = await db.TagFollows.ToListAsync();
        List<Tag> tags = await db.Tags.ToListAsync();
        List<User> users = await db.Users.ToListAsync();

        var followerAggregates = await db.TagFollows
            .GroupBy(e => e.TagId)
            .Select(g => new
            {
                TagId = g.Key,
                FollowerCount = g.Count(),
            })
            .ToListAsync();

        var followingAggregates = await db.TagFollows
            .GroupBy(e => e.UserId)
            .Select(g => new
            {
                UserId = g.Key,
                FollowingCount = g.Count(),
            })
            .ToListAsync();

        foreach (Tag t in tags)
        {
            var followerAggregate = followerAggregates.Where(x => x.TagId == t.Id).FirstOrDefault();

            t.FollowersCount = followerAggregate?.FollowerCount ?? 0;
        }

        foreach (User u in users)
        {
            var followingAggregate = followingAggregates.Where(x => x.UserId == u.Id).FirstOrDefault();

            u.TagFollowingsCount = followingAggregate?.FollowingCount ?? 0;
        }

        await db.SaveChangesAsync();
    }
}
