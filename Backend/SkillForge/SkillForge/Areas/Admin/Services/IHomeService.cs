using SkillForge.Models.DTOs.Home;

namespace SkillForge.Areas.Admin.Services;

public interface IHomeService
{
    Task<HomePageData> LoadPage(int? userId);
}