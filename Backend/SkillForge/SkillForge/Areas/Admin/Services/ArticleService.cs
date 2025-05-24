using SkillForge.Areas.Admin.Models;
using SkillForge.Areas.Admin.Models.Components.Common;
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

    public virtual async Task<ListingModel<Article>> CreatePendingArticlesListing(ListingModel listingQuery)
    {
        ListingModel<Article> model = new();
        model = InitializeListingModel(model, listingQuery);

        PaginatedList<Article> items = await repository.ListPending(model);

        model.Table = new Table<Article>(model, items)
            .AddRowAction("View", RequestMethod.Get, null)
            .SetFilterable(true)
            .SetOrderable(true)
            .SetSearchable(true)
            .AddPagination(true);

        return model;
    }
}
