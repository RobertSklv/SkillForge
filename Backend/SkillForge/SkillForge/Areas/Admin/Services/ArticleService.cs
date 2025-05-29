using SkillForge.Areas.Admin.Models;
using SkillForge.Areas.Admin.Models.Components.Grid;
using SkillForge.Areas.Admin.Models.DTOs;
using SkillForge.Areas.Admin.Repositories;
using SkillForge.Models.Database;

namespace SkillForge.Areas.Admin.Services;

public class ArticleService : CrudService<Article>, IArticleService
{
    private readonly IArticleRepository repository;

    public ArticleService(IArticleRepository repository)
        : base(repository)
    {
        this.repository = repository;
    }

    public override Table<Article> CreateEditRowAction(Table<Article> table)
    {
        // Creates a View action instead of Edit.

        return table.AddRowAction("View");
    }

    public virtual async Task<ListingModel<Article>> CreatePendingArticlesListing(ListingModel listingQuery)
    {
        ListingModel<Article> model = new();
        model = InitializeListingModel(model, listingQuery);
        model.ActionName = "Pending";

        PaginatedList<Article> items = await repository.ListPending(model);

        model.Table = new Table<Article>(model, items)
            .AddRowAction("Preview")
            .AddChainCall(CreateDeleteRowAction)
            .SetFilterable(true)
            .SetOrderable(true)
            .SetSearchable(true)
            .AddPagination(true);

        return model;
    }

    public async Task<bool> Approve(int id, int adminId)
    {
        Article article = await GetStrict(id);

        ArticleApproval approval = new()
        {
            ModeratorId = adminId
        };

        article.Approval = approval;

        return await Upsert(article);
    }

    public async Task UserCreate(ArticleCreateDTO model, int userId)
    {
        Article entity = new()
        {
            CategoryId = model.CategoryId,
            AuthorId = userId,
            Image = model.Image,
            Title = model.Title,
            Content = model.Content,
        };

        await repository.Upsert(entity);
    }
}
