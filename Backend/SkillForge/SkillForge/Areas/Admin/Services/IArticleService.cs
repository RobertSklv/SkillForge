using SkillForge.Areas.Admin.Models.DTOs;
using SkillForge.Areas.Admin.Models;
using SkillForge.Models.Database;

namespace SkillForge.Areas.Admin.Services;

public interface IArticleService : ICrudService<Article>
{
    Task<PaginatedList<Article>> ListPending(ListingModel listingModel);
}
