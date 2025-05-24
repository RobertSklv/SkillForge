namespace SkillForge.Models.Database;

public class ArticleTag : BaseEntity
{
    public Article? Article { get; set; }

    public int ArticleId { get; set; }

    public Tag? Tag { get; set; }

    public int TagId { get; set; }
}
