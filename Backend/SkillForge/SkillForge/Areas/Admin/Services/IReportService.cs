using SkillForge.Models.Database;
using SkillForge.Models.DTOs.Report;

namespace SkillForge.Areas.Admin.Services;

public interface IReportService
{
    ReportFormOptions GetFormOptions();

    string GetViolationTitle(Violation v);
}
