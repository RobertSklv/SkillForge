using SkillForge.Models.Database;

namespace SkillForge.Models.DTOs.Report;

public class ReportCreateFormData
{
    public int Id { get; set; }

    public Violation Reason { get; set; }

    public string? Message { get; set; }
}
