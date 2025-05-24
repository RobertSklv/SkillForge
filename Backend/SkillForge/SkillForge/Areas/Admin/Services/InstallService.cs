namespace SkillForge.Areas.Admin.Services;

public class InstallService : IInstallService
{
    public const string CONFIG_KEY_NAME = "AdminPanelInstallationKey";

    private readonly IConfiguration config;

    public InstallService(IConfiguration config)
    {
        this.config = config;
    }

    public bool Authenticate(string key)
    {
        string configKey = config[CONFIG_KEY_NAME] ?? throw new Exception("No installation key was found in the configuration file.");

        return configKey == key;
    }
}