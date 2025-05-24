using SkillForge.Areas.Admin.Models.Components.Common;

namespace SkillForge.Areas.Admin.Models.Components.Navigation;

public abstract class NavItem
{
    public string Name { get; set; }

    public BootstrapIcon? Icon { get; set; }

    public int SortOrder { get; set; }

    public bool IsActive { get; set; }
}