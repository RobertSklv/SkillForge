namespace SkillForge.Areas.Admin.Services;

public interface IInstallService
{
    bool Authenticate(string key);
}