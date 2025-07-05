namespace SkillForge.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
public class AdminNavLinkAttribute : AdminNavAttribute
{
    public string? Menu { get; set; }

    public string? ActionName { get; set; }

    public string? Route { get; set; }

    public AdminNavLinkAttribute(string displayedName, string? actionName = null)
        : base(displayedName)
    {
        ActionName = actionName;
    }
}