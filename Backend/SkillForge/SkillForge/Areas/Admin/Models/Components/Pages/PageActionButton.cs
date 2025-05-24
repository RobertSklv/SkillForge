using SkillForge.Areas.Admin.Models.Components.Common;

namespace SkillForge.Areas.Admin.Models.Components.Pages;

public class PageActionButton : Link
{
    public bool IsLink { get; set; }

    public bool AlignToLeft { get; set; }

    public int SortOrder { get; set; }
}
