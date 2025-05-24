using SkillForge.Areas.Admin.Models.DTOs;

namespace SkillForge.Areas.Admin.Models.Components.Grid;

public class MassActionContext
{
    public List<MassAction> Actions { get; set; } = new();

    public IListingModel ListingModel { get; set; }
}
