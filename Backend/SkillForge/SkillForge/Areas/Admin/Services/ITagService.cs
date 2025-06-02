using SkillForge.Areas.Admin.Models.DTOs;
using SkillForge.Models.Database;

namespace SkillForge.Areas.Admin.Services;

public interface ITagService : ICrudService<Tag>
{
    Task<List<Tag>> GetMostPopular();

    Task<List<TagLink>> GetMostPopularLinks();

    TagLink CreateTagLink(Tag tag);
}
