using SkillForge.Models.Database;

namespace SkillForge.Areas.Admin.Repositories;

public interface ICategoryRepository : ICrudRepository<Category>
{
    Task<List<Category>> GetPossibleParents(int id);

    Task<List<Category>> GetPossibleParents();
}
