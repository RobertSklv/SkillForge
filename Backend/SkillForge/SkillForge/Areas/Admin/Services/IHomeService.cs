using SkillForge.Areas.Admin.Models.DTOs;

namespace SkillForge.Areas.Admin.Services;

public interface IHomeService
{
    Task<HomePageData> LoadPage();
}