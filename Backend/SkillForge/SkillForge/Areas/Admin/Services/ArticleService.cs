using SkillForge.Areas.Admin.Models;
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

    public Task<PaginatedList<Article>> ListPending(ListingModel listingModel)
    {
        return repository.ListPending(listingModel);
    }
}
