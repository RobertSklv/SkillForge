using SkillForge.Models.Database;

namespace SkillForge.Models.DTOs.ArticleReport;

public class ArticleReportCreate
{
    public int ArticleId { get; set; }

    public Violation Reason { get; set; }

    public string? Message { get; set; }
}
