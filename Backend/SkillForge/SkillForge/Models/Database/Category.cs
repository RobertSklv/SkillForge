using SkillForge.Attributes;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SkillForge.Models.Database;

[SelectOption(LabelProperty = nameof(DisplayedName))]
public class Category : BaseEntity
{
    [StringLength(32, MinimumLength = 1)]
    [Column(TypeName = "varchar")]
    [Code]
    [TableColumn]
    public string Code { get; set; }

    [StringLength(32, MinimumLength = 1)]
    [Column(TypeName = "varchar")]
    [TableColumn]
    [Display(Name = "Displayed name")]
    public string DisplayedName { get; set; }

    [StringLength(64)]
    [Column(TypeName = "varchar")]
    [TableColumn]
    public string? Image { get; set; }

    [TableColumn]
    public string? Description { get; set; }

    public List<Article>? Articles { get; set; }
}
