using SkillForge.Areas.Admin.Models.DTOs;
using SkillForge.Models.Database;

namespace SkillForge.Areas.Admin.Services;

public interface ICategoryService : ICrudService<Category, CategoryVM>
{
    Task<List<Category>> GetPossibleParents(int id);

    Task<List<Category>> GetPossibleParents();
}
