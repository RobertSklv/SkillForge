using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SkillForge.Models.Database;

[Table("CommentReports")]
public class CommentReport : Report
{
    [DeleteBehavior(DeleteBehavior.NoAction)]
    public Comment? Comment { get; set; }

    public int CommentId { get; set; }
}
