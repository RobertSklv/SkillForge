using System.ComponentModel.DataAnnotations.Schema;
using SkillForge.Attributes;

namespace SkillForge.Models.Database;

public class AccountSuspension : BaseEntity
{
    [TableColumn]
    public User? User { get; set; }

    public int UserId { get; set; }

    [Column(TypeName = "tinyint")]
    [TableColumn]
    public Violation? Reason { get; set; }

    [TableColumn(Name = "Duration (days)")]
    public byte DurationDays { get; set; }

    public AdminUser? Moderator { get; set; }

    public int ModeratorId { get; set; }

    [TableColumn(Name = "Active")]
    public bool IsActive => DateTime.Compare(DateTime.Now, (DateTime)CreatedAt! + TimeSpan.FromDays(DurationDays)) < 0;
}
