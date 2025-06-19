using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using SkillForge.Attributes;

namespace SkillForge.Models.Database;

[Table("ArticleReports")]
public class ArticleReport : Report
{
    [DeleteBehavior(DeleteBehavior.NoAction)]
    public Article? Article { get; set; }

    [TableColumn(Name = "Article ID")]
    public int ArticleId { get; set; }
}
