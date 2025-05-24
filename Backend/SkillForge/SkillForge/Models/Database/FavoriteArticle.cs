namespace SkillForge.Models.Database;

public class FavoriteArticle : BaseEntity
{
    public User? User { get; set; }

    public int UserId { get; set; }

    public Article? Article { get; set; }

    public int ArticleId { get; set; }
}
