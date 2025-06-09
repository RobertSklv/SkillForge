using Microsoft.EntityFrameworkCore;

namespace SkillForge.Models.Database;

public class UserFollow : BaseEntity, IFollower, IFollowedUser
{
    [DeleteBehavior(DeleteBehavior.NoAction)]
    public User? Follower { get; set; }

    public int FollowerId { get; set; }

    [DeleteBehavior(DeleteBehavior.NoAction)]
    public User? FollowedUser { get; set; }

    public int FollowedUserId { get; set; }
}
