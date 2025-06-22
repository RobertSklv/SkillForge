using System.ComponentModel.DataAnnotations;
using SkillForge.Models.Database;

namespace SkillForge.Areas.Admin.Models.User;

public class SuspendUserFormData
{
    public int UserId { get; set; }

    public Violation Violation { get; set; }

    [Display(Name = "Duration (days)")]
    public byte DurationDays { get; set; }
}
