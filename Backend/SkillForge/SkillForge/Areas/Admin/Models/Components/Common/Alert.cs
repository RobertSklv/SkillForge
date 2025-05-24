namespace SkillForge.Areas.Admin.Models.Components.Common;

public class Alert
{
    public string Content { get; set; }

    public ColorClass Color { get; set; }

    public string ColorAsString => Color.ToString().ToLower();
}
