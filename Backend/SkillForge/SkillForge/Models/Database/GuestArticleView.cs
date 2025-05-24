using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SkillForge.Models.Database;

[Table("GuestArticleViews")]
public class GuestArticleView : ArticleView
{
    [StringLength(32)]
    public string GuestId { get; set; }
}
