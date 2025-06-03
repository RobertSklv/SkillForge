using SkillForge.Attributes;

namespace SkillForge.Models.Database;

[ManyToManyEntity(nameof(ArticleId), nameof(TagId))]
public class ArticleTag : BaseEntity
{
    public Article? Article { get; set; }

    public int ArticleId { get; set; }

    public Tag? Tag { get; set; }

    public int TagId { get; set; }
}
