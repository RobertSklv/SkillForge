using SkillForge.Areas.Admin.Models.Components.Grid;
using SkillForge.Areas.Admin.Models.DTOs;
using SkillForge.Areas.Admin.Models;
using SkillForge.Areas.Admin.Repositories;
using SkillForge.Models.Database;
using SkillForge.Models.DTOs.Report;
using SkillForge.Areas.Admin.Models.Components.Common;
using SkillForge.Exceptions;

namespace SkillForge.Areas.Admin.Services;

public class UserReportService : CrudService<UserReport>, IUserReportService
{
    private readonly IUserReportRepository repository;
    private readonly IUserService userService;

    public UserReportService(IUserReportRepository repository, IUserService userService)
        : base(repository)
    {
        this.repository = repository;
        this.userService = userService;
    }

    public override Table<UserReport> CreateEditRowAction(Table<UserReport> table)
    {
        // Creates a View action instead of Edit.

        return table.AddRowAction("View");
    }

    public override Table<UserReport> CreateDeleteRowAction(Table<UserReport> table)
    {
        // Creates a Close action instead of Delete.

        return table.AddRowAction("Close", customizationCallback: a =>
        {
            return a.SetColor(ColorClass.Danger);
        });
    }

    public override async Task<Table<UserReport>> CreateListingTable(ListingModel<UserReport> listingModel, PaginatedList<UserReport> items)
    {
        return (await base.CreateListingTable(listingModel, items))
            .SetSelectableOptionsSource(nameof(UserReport.Reporter), await userService.GetAll())
            .AddMassAction("MassClose", "Close selected");
    }

    public async Task<ListingModel<UserReport>> CreateClosedReportsListing(ListingModel listingQuery)
    {
        ListingModel<UserReport> model = new();
        model = InitializeListingModel(model, listingQuery);
        model.ActionName = "Closed";

        PaginatedList<UserReport> items = await repository.ListClosed(model);

        model.Table = new Table<UserReport>(model, items)
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
        if (form.Name == null) throw new ArgumentException("The Name field is required");

        User user = await userService.GetByName(form.Name) ?? throw new RecordNotFoundException($"User not found.");

        UserReport entity = new()
        {
            ReportedUserId = user.Id,
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
