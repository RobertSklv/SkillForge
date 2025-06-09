using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using SkillForge.Attributes;

namespace SkillForge.Models.Database;

public class Tag : BaseEntity
{
    [StringLength(32, MinimumLength = 1)]
    [Column(TypeName = "varchar")]
    [Code]
    [TableColumn]
    public string Name { get; set; }

    [StringLength(128)]
    [Column(TypeName = "varchar")]
    [TableColumn]
    public string? Description { get; set; }

    public int FollowersCount { get; set; }

    public int ArticlesCount { get; set; }

    public List<TagFollow>? Followers { get; set; }

    public List<ArticleTag>? Articles { get; set; }
}
