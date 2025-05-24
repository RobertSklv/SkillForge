using SkillForge.Attributes;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SkillForge.Models.Database;

public class Category : BaseEntity
{
    [StringLength(32, MinimumLength = 1)]
    [Column(TypeName = "varchar")]
    [Code]
    public string Code { get; set; }

    [StringLength(32, MinimumLength = 1)]
    [Column(TypeName = "varchar")]
    public string DisplayedName { get; set; }

    public List<Article>? Articles { get; set; }
}
