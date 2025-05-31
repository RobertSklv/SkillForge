namespace SkillForge.Models.Database;

public class CommentRating : BaseEntity
{
    public User? User { get; set; }

    public int UserId { get; set; }

    public Comment? Comment { get; set; }

    public int CommentId { get; set; }

    public short Rate { get; set; }
}
