using SkillForge.Models.Database;
using SkillForge.Models.DTOs.Tag;

namespace SkillForge.Areas.Admin.Services;

public interface ITagService : ICrudService<Tag>
{
    Task<Tag?> GetByName(string name);

    Task<List<Tag>> GetByNames(List<string> names);

    Task<List<Tag>> GetByNamesAndCreateNonexisting(List<string> names);

    Task<List<Tag>> GetMostFollowed();

    Task<bool> IsFollowedByUser(int userId, int tagId);

    Task<List<Tag>> Search(string? phrase, List<string>? exclude);

    Task<List<TagLink>> SearchLinks(string? phrase, List<string>? exclude);

    Task<List<TagLink>> GetMostFollowedLinks();

    Task<TagPageData> LoadPage(Tag tag, int? userId = null);
}
