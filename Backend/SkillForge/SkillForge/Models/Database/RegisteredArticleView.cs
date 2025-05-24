using System.ComponentModel.DataAnnotations.Schema;

namespace SkillForge.Models.Database;

[Table("RegisteredArticleViews")]
public class RegisteredArticleView : ArticleView
{
    public User? User { get; set; }

    public int UserId { get; set; }
}
