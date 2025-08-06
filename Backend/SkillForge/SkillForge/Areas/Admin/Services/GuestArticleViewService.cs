using SkillForge.Areas.Admin.Models;
using SkillForge.Areas.Admin.Models.Components.Grid;
using SkillForge.Areas.Admin.Models.DTOs;
using SkillForge.Areas.Admin.Repositories;
using SkillForge.Models.Database;

namespace SkillForge.Areas.Admin.Services;

public class GuestArticleViewService : CrudService<GuestArticleView>, IGuestArticleViewService
{
    private readonly IArticleService articleService;

    public GuestArticleViewService(IGuestArticleViewRepository repository, IArticleService articleService)
        : base(repository)
    {
        this.articleService = articleService;
    }

    public override async Task<Table<GuestArticleView>> CreateListingTable(ListingModel<GuestArticleView> listingModel, PaginatedList<GuestArticleView> items)
    {
        return (await base.CreateListingTable(listingModel, items))
            .RemoveColumn(nameof(BaseEntity.UpdatedAt))
            .OverrideColumnName(nameof(BaseEntity.CreatedAt), "Viewed at")
            .SetSelectableOptionsSource(nameof(GuestArticleView.Article), await articleService.GetAll());
    }

    public override Table<GuestArticleView> CreateEditRowAction(Table<GuestArticleView> table)
    {
        return table;
    }

    public override Table<GuestArticleView> CreateDeleteRowAction(Table<GuestArticleView> table)
    {
        return table;
    }
}
