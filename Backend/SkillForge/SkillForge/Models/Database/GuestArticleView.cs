using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SkillForge.Attributes;

namespace SkillForge.Models.Database;

[Table("GuestArticleViews")]
public class GuestArticleView : ArticleView
{
    [StringLength(36)]
    [Column(TypeName = "varchar")]
    [TableColumn(Name = "Guest ID")]
    public string GuestId { get; set; }
}
