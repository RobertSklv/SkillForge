namespace SkillForge.Models.Database;

public class TagFollow : BaseEntity
{
    public User? User { get; set; }

    public int UserId { get; set; }

    public Tag? Tag { get; set; }

    public int TagId { get; set; }
}
