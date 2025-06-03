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

    public Task<Tag?> GetByName(string name)
    {
        return repository.GetByName(name);
    }

    public Task<List<Tag>> GetByNames(List<string> names)
    {
        return repository.GetByNames(names);
    }

    public async Task<List<Tag>> GetByNamesAndCreateNonexisting(List<string> names)
    {
        List<Tag> tags = await GetByNames(names);

        List<string> nonexsistent = names
            .Where(n => !tags.ConvertAll(t => t.Name).Contains(n))
            .ToList();

        List<Tag> newTags = new();

        foreach (string name in nonexsistent)
        {
            newTags.Add(new Tag
            {
                Name = name
            });
        }

        await UpsertMultiple(newTags);

        tags.AddRange(newTags);

        return tags;
    }

    public Task<List<Tag>> GetMostPopular()
    {
        return repository.GetMostFollowed();
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

    public Task<List<Tag>> Search(string? phrase, List<string>? exclude)
    {
        return repository.Search(phrase, exclude);
    }

    public async Task<List<TagLink>> SearchLinks(string? phrase, List<string>? exclude)
    {
        return (await repository.Search(phrase, exclude)).ConvertAll(CreateTagLink);
    }
}
