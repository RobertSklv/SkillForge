using System.ComponentModel.DataAnnotations.Schema;
using SkillForge.Attributes;

namespace SkillForge.Models.Database;

[Table("ArticleViews")]
public abstract class ArticleView : BaseEntity
{
    [TableColumn]
    public Article? Article { get; set; }

    public int ArticleId { get; set; }
}
