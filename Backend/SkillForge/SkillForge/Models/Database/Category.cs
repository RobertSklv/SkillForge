using SkillForge.Attributes;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace SkillForge.Models.Database;

[SelectOption(LabelProperty = nameof(DisplayedName), UndefinedLabel = "")]
public class Category : BaseEntity
{
    [DeleteBehavior(DeleteBehavior.NoAction)]
    [TableColumn]
    public Category? Parent { get; set; }

    public int? ParentId { get; set; }

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
    [StringLength(1024)]
    public string? Description { get; set; }

    public List<Article>? Articles { get; set; }

    public List<Category>? SubCategories { get; set; }

    public int ArticlesCount { get; set; }
}
