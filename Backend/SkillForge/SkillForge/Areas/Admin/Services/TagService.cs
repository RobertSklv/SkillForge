using SkillForge.Areas.Admin.Repositories;
using SkillForge.Exceptions;
using SkillForge.Models.Database;
using SkillForge.Models.DTOs.Article;
using SkillForge.Models.DTOs.Tag;
using SkillForge.Models.DTOs.User;
using SkillForge.Services;

namespace SkillForge.Areas.Admin.Services;

public class TagService : CrudService<Tag>, ITagService
{
    private readonly ITagRepository repository;
    private readonly IUserService userService;
    private readonly IFrontendService frontendService;
    private readonly IUserFeedService userFeedService;

    public TagService(
        ITagRepository repository,
        IUserService userService,
        IFrontendService frontendService,
        IUserFeedService userFeedService)
        : base(repository)
    {
        this.repository = repository;
        this.userService = userService;
        this.frontendService = frontendService;
        this.userFeedService = userFeedService;
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

    public Task<List<Tag>> GetMostFollowed()
    {
        return repository.GetMostFollowed();
    }

    public Task<bool> IsFollowedByUser(int userId, int tagId)
    {
        return repository.IsFollowedByUser(userId, tagId);
    }

    public async Task Follow(int userId, string tagName)
    {
        Tag? tag = await GetByName(tagName) ?? throw new RecordNotFoundException($"Tag '{tagName}' does not exist.");

        TagFollow? followRecord = await repository.GetFollowRecord(userId, tag.Id);

        if (followRecord != null)
        {
            throw new AlreadyFollowingException();
        }

        TagFollow follow = new()
        {
            UserId = userId,
            TagId = tag.Id
        };

        tag.FollowersCount++;

        await repository.SaveFollowRecord(follow);
    }

    public async Task Unfollow(int userId, string tagName)
    {
        Tag? tag = await GetByName(tagName) ?? throw new RecordNotFoundException($"Tag '{tagName}' does not exist.");

        TagFollow? followRecord = await repository.GetFollowRecord(userId, tag.Id) ?? throw new AlreadyNotFollowingException();

        tag.FollowersCount--;

        await repository.DeleteFollowRecord(followRecord);
    }

    public async Task<List<TagLink>> GetMostFollowedLinks()
    {
        List<Tag> tags = await GetMostFollowed();

        return tags.ConvertAll(frontendService.CreateTagLink);
    }

    public async Task<TagPageData> LoadPage(string name, int? userId = null)
    {
        Tag? tag = await GetByName(name) ?? throw new RecordNotFoundException($"Tag '{name}' does not exist.");

        List<TopArticleItem> topArticles = await userFeedService.GetTopArticlesByTag(tag.Id, 5);
        List<UserListItem> latestFollowers = await userFeedService.GetLatestTagFollowers(tag.Id, userId, 2);

        return new TagPageData
        {
            Name = tag.Name,
            Description = tag.Description,
            ArticlesCount = tag.ArticlesCount,
            FollowersCount = tag.FollowersCount,
            IsFollowedByCurrentUser = userId != null && await IsFollowedByUser((int)userId, tag.Id),
            LatestFollowers = latestFollowers,
            TopArticles = topArticles,
        };
    }

    public Task<List<Tag>> Search(string? phrase, List<string>? exclude)
    {
        return repository.Search(phrase, exclude);
    }

    public async Task<List<TagLink>> SearchLinks(string? phrase, List<string>? exclude)
    {
        return (await repository.Search(phrase, exclude)).ConvertAll(frontendService.CreateTagLink);
    }
}
