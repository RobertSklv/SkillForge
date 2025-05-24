using SkillForge.Areas.Admin.Models.Components.Common;
using SkillForge.Areas.Admin.Models.Components.Grid;

namespace SkillForge.Areas.Admin.Models.DTOs;

public interface IListingModel : IRouteElement
{
    string? OrderBy { get; set; }

    string? Direction { get; set; }

    int? Page { get; set; }

    int? PageSize { get; set; }

    Dictionary<string, TableFilter>? Filters { get; set; }

    string? SearchPhrase { get; set; }

    Dictionary<string, string?> GenerateListingQuery();

    void CopyFrom(IListingModel? listingModel);

    IListingModel Clone();
}
