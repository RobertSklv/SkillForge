using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SkillForge.Models.Database;

[Table("GuestArticleViews")]
public class GuestArticleView : ArticleView
{
    [StringLength(36)]
    [Column(TypeName = "varchar")]
    public string GuestId { get; set; }
}
