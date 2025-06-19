using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SkillForge.Models.Database;

[Table("UserReports")]
public class UserReport : Report
{
    [DeleteBehavior(DeleteBehavior.NoAction)]
    public User? ReportedUser { get; set; }

    public int ReportedUserId { get; set; }
}
