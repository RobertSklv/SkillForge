using SkillForge.Areas.Admin.Models.DTOs;
using SkillForge.Areas.Admin.Models;
using SkillForge.Models.Database;

namespace SkillForge.Areas.Admin.Repositories;

public interface IArticleRepository : ICrudRepository<Article>
{
    Task<PaginatedList<Article>> ListPending(ListingModel listingModel, Func<IQueryable<Article>, IQueryable<Article>>? queryCallback = null);
}
