using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using SkillForge.Attributes;

namespace SkillForge.Models.Database;

[Table("Reports")]
public abstract class Report : BaseEntity
{
    [TableColumn]
    public User? Reporter { get; set; }

    public int ReporterId { get; set; }

    [TableColumn]
    [Column(TypeName = "tinyint")]
    public Violation Reason { get; set; }

    [TableColumn]
    [StringLength(256)]
    [Column(TypeName = "varchar")]
    public string? Message { get; set; }

    public bool IsClosed { get; set; }
}
