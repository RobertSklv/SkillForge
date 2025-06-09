namespace SkillForge.Models.Database;

public interface IFollower : IFollowEntity
{
    User? Follower { get; }

    int FollowerId { get; }
}
