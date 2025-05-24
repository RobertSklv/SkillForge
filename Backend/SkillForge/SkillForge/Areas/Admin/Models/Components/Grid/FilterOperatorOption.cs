namespace SkillForge.Areas.Admin.Models.Components.Grid;

public class FilterOperatorOption
{
    public string Value { get; set; }

    public string Label { get; set; }

    public FilterOperatorOption(string value, string label)
    {
        Value = value;
        Label = label;
    }
}
