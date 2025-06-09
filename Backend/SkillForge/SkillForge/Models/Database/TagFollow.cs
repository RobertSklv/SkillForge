namespace SkillForge.Models.Database;

public class TagFollow : BaseEntity, IFollower
{
    public User? User { get; set; }

    public int UserId { get; set; }

    public Tag? Tag { get; set; }

    public int TagId { get; set; }

    public User? Follower => User;

    public int FollowerId => UserId;
}