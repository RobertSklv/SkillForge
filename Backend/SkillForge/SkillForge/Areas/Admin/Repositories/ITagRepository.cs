using SkillForge.Models.Database;

namespace SkillForge.Areas.Admin.Repositories;

public interface ITagRepository : ICrudRepository<Tag>
{
    Task<Tag?> GetByName(string name);

    Task<List<Tag>> GetByNames(List<string> names);

    Task<List<Tag>> GetMostFollowed();

    Task<List<Tag>> Search(string? phrase, List<string>? exclude);
}
