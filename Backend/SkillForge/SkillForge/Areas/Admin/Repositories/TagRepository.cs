using Microsoft.EntityFrameworkCore;
using SkillForge.Areas.Admin.Services;
using SkillForge.Data;
using SkillForge.Models.Database;

namespace SkillForge.Areas.Admin.Repositories;

public class TagRepository : CrudRepository<Tag>, ITagRepository
{
    public override DbSet<Tag> DbSet => db.Tags;

    public TagRepository(
        AppDbContext db,
        IEntityFilterService filterService,
        IEntitySortService sortService,
        IEntitySearchService searchService)
        : base(db, filterService, sortService, searchService)
    {
    }

    public Task<Tag?> GetByName(string name)
    {
        return DbSet.FirstOrDefaultAsync(e => e.Name == name);
    }

    public Task<List<Tag>> GetByNames(List<string> names)
    {
        return DbSet
            .Where(e => names.Contains(e.Name))
            .ToListAsync();
    }

    public Task<List<Tag>> GetMostFollowed()
    {
        return DbSet
            .Where(e => e.ArticlesCount > 0)
            .OrderByDescending(e => e.FollowersCount)
            .Take(8)
            .ToListAsync();
    }

    public Task<TagFollow?> GetFollowRecord(int userId, int tagId)
    {
        return db.TagFollows.FirstOrDefaultAsync(e => e.UserId == userId && e.TagId == tagId);
    }

    public Task<bool> IsFollowedByUser(int userId, int tagId)
    {
        return db.TagFollows.AnyAsync(tf => tf.UserId == userId && tf.TagId == tagId);
    }

    public Task SaveFollowRecord(TagFollow followRecord)
    {
        db.TagFollows.Add(followRecord);

        return db.SaveChangesAsync();
    }

    public Task DeleteFollowRecord(int userId, int tagId)
    {
        return db.TagFollows
            .Where(e => e.UserId == userId && e.TagId == tagId)
            .ExecuteDeleteAsync();
    }

    public Task DeleteFollowRecord(TagFollow followRecord)
    {
        db.TagFollows.Remove(followRecord);

        return db.SaveChangesAsync();
    }

    public Task<List<TagFollow>> GetFollowers(int tagId, int batchIndex, int batchSize)
    {
        return db.TagFollows
            .Include(e => e.User)
            .Where(e => e.TagId == tagId)
            .OrderByDescending(e => e.CreatedAt)
            .Skip(batchIndex * batchSize)
            .Take(batchSize)
            .ToListAsync();
    }

    public Task<List<Tag>> Search(string? phrase, List<string>? exclude)
    {
        IQueryable<Tag> query = DbSet;

        if (!string.IsNullOrEmpty(phrase))
        {
            query = query.Where(e => e.Name.Contains(phrase) || e.Name.StartsWith(phrase) || e.Name.EndsWith(phrase));
        }

        if (exclude != null && exclude.Count > 0)
        {
            query = query.Where(e => !exclude.Contains(e.Name));
        }

        return query
            .Where(e => e.ArticlesCount > 0)
            .OrderByDescending(e => e.ArticlesCount)
            .Take(6)
            .ToListAsync();
    }

    public Task<List<TagFollow>> GetLatestFollowers(int tagId, int count)
    {
        return db.TagFollows
            .Include(e => e.User)
            .Where(e => e.TagId == tagId)
            .OrderByDescending(e => e.CreatedAt)
            .Take(count)
            .ToListAsync();
    }

    public Task<List<TagFollow>> GetLatestFollowers(string tagName, int count)
    {
        return db.TagFollows
            .Include(e => e.User)
            .Include(e => e.Tag)
            .Where(e => e.Tag!.Name == tagName)
            .OrderByDescending(e => e.CreatedAt)
            .Take(count)
            .ToListAsync();
    }
}
