using System.ComponentModel.DataAnnotations.Schema;

namespace SkillForge.Models.Database;

[Table("ArticleViews")]
public abstract class ArticleView : BaseEntity
{
    public Article? Article { get; set; }

    public int ArticleId { get; set; }
}
