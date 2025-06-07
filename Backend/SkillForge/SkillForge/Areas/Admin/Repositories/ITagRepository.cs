using SkillForge.Models.Database;

namespace SkillForge.Areas.Admin.Repositories;

public interface ITagRepository : ICrudRepository<Tag>
{
    Task<Tag?> GetByName(string name);

    Task<List<Tag>> GetByNames(List<string> names);

    Task<List<Tag>> GetMostFollowed();

    Task<TagFollow?> GetFollowRecord(int userId, int tagId);

    Task<bool> IsFollowedByUser(int userId, int tagId);

    Task SaveFollowRecord(TagFollow followRecord);

    Task DeleteFollowRecord(int userId, int tagId);

    Task DeleteFollowRecord(TagFollow followRecord);

    Task<List<Tag>> Search(string? phrase, List<string>? exclude);

    Task<List<TagFollow>> GetLatestFollowers(int tagId, int count);

    Task<List<TagFollow>> GetLatestFollowers(string tagName, int count);
}
