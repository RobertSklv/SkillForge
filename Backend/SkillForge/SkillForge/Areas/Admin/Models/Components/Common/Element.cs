namespace SkillForge.Areas.Admin.Models.Components.Common;

public class Element
{
    public string Id { get; set; }

    public List<string> ClassList { get; set; } = new();

    public string? Content { get; set; }

    public string? Title { get; set; }

    public List<HtmlAttribute> AttributeList { get; set; } = new();

    public ColorClass Color { get; set; }

    public string ColorAsString => Color.ToString().ToLower();

    public string? Class => ClassList.Any() ? string.Join(' ', ClassList) : null;

    public string? Attributes => AttributeList.Any() ? string.Join(' ', AttributeList) : null;
}
