using SkillForge.Areas.Admin.Models.DTOs;
using SkillForge.Areas.Admin.Repositories;
using SkillForge.Models.Database;

namespace SkillForge.Areas.Admin.Services;

public class TagService : CrudService<Tag>, ITagService
{
    private readonly ITagRepository repository;

    public TagService(ITagRepository repository)
        : base(repository)
    {
        this.repository = repository;
    }

    public Task<List<Tag>> GetMostPopular()
    {
        return repository.GetMostPopular();
    }

    public async Task<List<TagLink>> GetMostPopularLinks()
    {
        List<Tag> tags = await GetMostPopular();

        return tags.ConvertAll(CreateTagLink);
    }

    public TagLink CreateTagLink(Tag tag)
    {
        return new TagLink
        {
            Name = tag.Name,
            Description = tag.Description,
        };
    }
}
