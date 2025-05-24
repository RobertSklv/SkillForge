namespace SkillForge.Attributes;

public abstract class AdminNavAttribute : Attribute
{
    public readonly string displayedName;

    public string IconClass { get; set; }

    public int SortOrder { get; set; } = 1;

    public AdminNavAttribute(string displayedName)
    {
        this.displayedName = displayedName;
    }
}