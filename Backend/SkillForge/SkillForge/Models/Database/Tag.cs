using SkillForge.Attributes;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SkillForge.Models.Database;

public class Tag : BaseEntity
{
    [StringLength(32, MinimumLength = 1)]
    [Column(TypeName = "varchar")]
    public string Name { get; set; }
}
