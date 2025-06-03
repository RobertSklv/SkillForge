using SkillForge.Models.Database;
using SkillForge.Models.DTOs.Tag;

namespace SkillForge.Areas.Admin.Services;

public interface ITagService : ICrudService<Tag>
{
    Task<Tag?> GetByName(string name);

    Task<List<Tag>> GetByNames(List<string> names);

    Task<List<Tag>> GetByNamesAndCreateNonexisting(List<string> names);

    Task<List<Tag>> GetMostPopular();

    Task<List<Tag>> Search(string? phrase, List<string>? exclude);

    Task<List<TagLink>> SearchLinks(string? phrase, List<string>? exclude);

    Task<List<TagLink>> GetMostPopularLinks();

    TagLink CreateTagLink(Tag tag);
}
