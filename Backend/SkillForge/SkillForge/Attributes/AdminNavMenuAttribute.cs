namespace SkillForge.Attributes;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class AdminNavMenuAttribute : AdminNavAttribute
{
    public readonly string code;

    public AdminNavMenuAttribute(string code, string displayedName)
        : base(displayedName)
    {
        this.code = code;
    }
}