namespace SkillForge.Areas.Admin.Models.Components.Common;

public class HtmlAttribute
{
    public string Name { get; set; }

    public object? Value { get; set; }

    public bool IsProperty { get; set; }

    public override string ToString()
    {
        return IsProperty
            ? Name
            : $"{Name}={Value?.ToString()}";
    }
}