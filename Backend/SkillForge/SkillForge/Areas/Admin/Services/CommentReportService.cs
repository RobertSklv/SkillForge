using SkillForge.Areas.Admin.Models.Components.Grid;
using SkillForge.Areas.Admin.Models.DTOs;
using SkillForge.Areas.Admin.Models;
using SkillForge.Areas.Admin.Repositories;
using SkillForge.Models.Database;
using SkillForge.Models.DTOs.Report;
using SkillForge.Areas.Admin.Models.Components.Common;

namespace SkillForge.Areas.Admin.Services;

public class CommentReportService : CrudService<CommentReport>, ICommentReportService
{
    private readonly ICommentReportRepository repository;
    private readonly IUserService userService;

    public CommentReportService(ICommentReportRepository repository, IUserService userService)
        : base(repository)
    {
        this.repository = repository;
        this.userService = userService;
    }

    public override Table<CommentReport> CreateEditRowAction(Table<CommentReport> table)
    {
        // Creates a View action instead of Edit.

        return table.AddRowAction("View");
    }

    public override Table<CommentReport> CreateDeleteRowAction(Table<CommentReport> table)
    {
        // Creates a Close action instead of Delete.

        return table.AddRowAction("Close", customizationCallback: a =>
        {
            return a.SetColor(ColorClass.Danger);
        });
    }

    public override async Task<Table<CommentReport>> CreateListingTable(ListingModel<CommentReport> listingModel, PaginatedList<CommentReport> items)
    {
        return (await base.CreateListingTable(listingModel, items))
            .SetSelectableOptionsSource(nameof(CommentReport.Reporter), await userService.GetAll())
            .AddMassAction("MassClose", "Close selected");
    }

    public async Task<ListingModel<CommentReport>> CreateClosedReportsListing(ListingModel listingQuery)
    {
        ListingModel<CommentReport> model = new();
        model = InitializeListingModel(model, listingQuery);
        model.ActionName = "Closed";

        PaginatedList<CommentReport> items = await repository.ListClosed(model);

        model.Table = new Table<CommentReport>(model, items)
            .AddRowAction("View")
            .SetAdjustablePageSize(true)
            .SetFilterable(true)
            .SetOrderable(true)
            .SetSearchable(true)
            .AddPagination(true);

        return model;
    }

    public async Task Create(int userId, ReportCreateFormData form)
    {
        CommentReport entity = new()
        {
            CommentId = form.Id,
            Reason = form.Reason,
            Message = form.Message,
            ReporterId = userId,
        };

        await Upsert(entity);
    }

    public async Task<bool> Close(int id)
    {
        return await repository.Close(id) > 0;
    }

    public async Task<bool> MassClose(List<int> ids)
    {
        return await repository.MassClose(ids) > 0;
    }
}
