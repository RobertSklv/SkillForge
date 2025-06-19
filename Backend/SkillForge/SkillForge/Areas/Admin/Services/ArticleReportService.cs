using SkillForge.Areas.Admin.Models;
using SkillForge.Areas.Admin.Models.Components.Grid;
using SkillForge.Areas.Admin.Models.DTOs;
using SkillForge.Areas.Admin.Repositories;
using SkillForge.Models.Database;
using SkillForge.Models.DTOs.ArticleReport;

namespace SkillForge.Areas.Admin.Services;

public class ArticleReportService : CrudService<ArticleReport>, IArticleReportService
{
    private readonly IArticleReportRepository repository;
    private readonly IUserService userService;

    public ArticleReportService(IArticleReportRepository repository, IUserService userService)
        : base(repository)
    {
        this.repository = repository;
        this.userService = userService;
    }

    public override Table<ArticleReport> CreateEditRowAction(Table<ArticleReport> table)
    {
        // Creates a View action instead of Edit.

        return table.AddRowAction("View");
    }

    public override Table<ArticleReport> CreateDeleteRowAction(Table<ArticleReport> table)
    {
        // Creates a Close action instead of Delete.

        return table.AddRowAction("Close");
    }

    public override async Task<Table<ArticleReport>> CreateListingTable(ListingModel<ArticleReport> listingModel, PaginatedList<ArticleReport> items)
    {
        return (await base.CreateListingTable(listingModel, items))
            .SetSelectableOptionsSource(nameof(ArticleReport.Reporter), await userService.GetAll())
            .AddMassAction("MassClose", "Close selected");
    }

    public async Task<ListingModel<ArticleReport>> CreateClosedReportsListing(ListingModel listingQuery)
    {
        ListingModel<ArticleReport> model = new();
        model = InitializeListingModel(model, listingQuery);
        model.ActionName = "Closed";

        PaginatedList<ArticleReport> items = await repository.ListClosed(model);

        model.Table = new Table<ArticleReport>(model, items)
            .AddRowAction("View")
            .SetAdjustablePageSize(true)
            .SetFilterable(true)
            .SetOrderable(true)
            .SetSearchable(true)
            .AddPagination(true);

        return model;
    }

    public ArticleReportCreateFormOptions GetFormOptions()
    {
        return new ArticleReportCreateFormOptions
        {
            ViolationOptions = Enum.GetValues<Violation>().ToList().ConvertAll(v => new EntityOption
            {
                Value = (int)v,
                Label = GetViolationTitle(v)
            })
        };
    }

    public string GetViolationTitle(Violation v)
    {
        return v switch
        {
            Violation.Spam => "Spam",
            Violation.HarrasmentAbuse => "Harrasment or Abuse",
            Violation.HateSpeechDiscrimination => "Hate Speech or Discrimination",
            Violation.Misinformation => "Misinformation",
            Violation.InappropriateContent => "Inappropriate Content",
            Violation.OffTopic => "Off-topic",
            Violation.CopyrightInfringement => "Copyright Infringement",
            Violation.Impersonation => "Impersonation",
            Violation.Other => "Other",
            _ => throw new InvalidOperationException($"Unsupported violation type: {v}")
        };
    }

    public async Task Create(int userId, ArticleReportCreate form)
    {
        ArticleReport entity = new()
        {
            ArticleId = form.ArticleId,
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
