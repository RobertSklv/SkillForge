using SkillForge.Areas.Admin.Models.DTOs;
using SkillForge.Models.Database;
using SkillForge.Models.DTOs.Report;

namespace SkillForge.Areas.Admin.Services;

public class ReportService : IReportService
{
    public ReportFormOptions GetFormOptions()
    {
        return new ReportFormOptions
        {
            ViolationOptions = Enum.GetValues<Violation>().ToList().ConvertAll(v => new EntityOption
            {
                Value = (int)v,
                Label = GetViolationTitle(v)
            })
        };
    }

    public string GetViolationTitle(Violation v)
    {
        return v switch
        {
            Violation.Spam => "Spam",
            Violation.HarrasmentAbuse => "Harrasment or Abuse",
            Violation.HateSpeechDiscrimination => "Hate Speech or Discrimination",
            Violation.Misinformation => "Misinformation",
            Violation.InappropriateContent => "Inappropriate Content",
            Violation.OffTopic => "Off-topic",
            Violation.CopyrightInfringement => "Copyright Infringement",
            Violation.Other => "Other",
            _ => throw new InvalidOperationException($"Unsupported violation type: {v}")
        };
    }
}
