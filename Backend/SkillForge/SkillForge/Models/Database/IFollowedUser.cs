namespace SkillForge.Models.Database;

public interface IFollowedUser : IFollowEntity
{
    User? FollowedUser { get; }

    int FollowedUserId { get; }
}
