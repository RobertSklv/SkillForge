namespace SkillForge.Areas.Admin.Models.Components.Pages;

public class SidebarLinkGroup : List<SidebarLink>
{
    public string? ActiveLinkId { get; set; }

    public bool IsActive(string linkId)
    {
        if (ActiveLinkId != null)
        {
            return linkId == ActiveLinkId;
        }

        return false;
    }
}
