namespace SkillForge.Models.Database;

public class ArticleApproval : BaseEntity
{
    public AdminUser? Moderator { get; set; }

    public int ModeratorId { get; set; }
}
