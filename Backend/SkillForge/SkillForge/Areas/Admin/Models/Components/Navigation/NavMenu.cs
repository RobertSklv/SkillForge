namespace SkillForge.Areas.Admin.Models.Components.Navigation;

public class NavMenu : NavItem
{
    public string Code { get; set; }

    public List<NavLink> Links { get; set; } = new();
}