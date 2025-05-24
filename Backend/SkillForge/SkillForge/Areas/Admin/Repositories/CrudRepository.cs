using Microsoft.EntityFrameworkCore;
using SkillForge.Areas.Admin.Models;
using SkillForge.Areas.Admin.Models.DTOs;
using SkillForge.Areas.Admin.Services;
using SkillForge.Data;
using SkillForge.Models.Database;

namespace SkillForge.Areas.Admin.Repositories;

public abstract class CrudRepository<TEntity> : ICrudRepository<TEntity>
    where TEntity : class, IBaseEntity
{
    public abstract DbSet<TEntity> DbSet { get; }

    protected readonly AppDbContext db;
    protected readonly IEntityFilterService filterService;
    protected readonly IEntitySortService sortService;
    protected readonly IEntitySearchService searchService;

    public CrudRepository(
        AppDbContext db,
        IEntityFilterService filterService,
        IEntitySortService sortService,
        IEntitySearchService searchService)
    {
        this.db = db;
        this.filterService = filterService;
        this.sortService = sortService;
        this.searchService = searchService;
    }

    public virtual IQueryable<TEntity> List(DbSet<TEntity> dbSet)
    {
        return dbSet;
    }

    public virtual IQueryable<TEntity> GetIncludes(IQueryable<TEntity> query)
    {
        return query;
    }

    public virtual Task<TEntity?> Get(int id)
    {
        return GetIncludes(DbSet).FirstOrDefaultAsync(e => e.Id == id);
    }

    public virtual async Task<TEntity> GetStrict(int id)
    {
        return await Get(id) ?? throw new Exception($"No {typeof(TEntity).Name} record with ID {id} was found.");
    }

    public virtual Task<List<TEntity>> GetAll()
    {
        return GetIncludes(DbSet).ToListAsync();
    }

    public virtual Task<List<TEntity>> GetByIds(IEnumerable<int> ids)
    {
        return GetIncludes(DbSet)
            .Where(e => ids.Contains(e.Id))
            .ToListAsync();
    }

    public virtual async Task<int> Upsert(TEntity entity)
    {
        if (entity.Id > 0)
        {
            return await Update(entity);
        }

        await DbSet.AddAsync(entity);

        return await db.SaveChangesAsync();
    }

    public virtual async Task<int> Update(TEntity entity)
    {
        DbSet.Update(entity);

        return await db.SaveChangesAsync();
    }

    public virtual async Task<int> SaveMultiple(List<TEntity> entities)
    {
        DbSet.AddRange(entities);

        return await db.SaveChangesAsync();
    }

    public virtual async Task<int> Delete(int id)
    {
        int deleteResult = await DbSet
            .Where(c => c.Id == id)
            .Take(1)
            .ExecuteDeleteAsync();

        return deleteResult;
    }

    public virtual async Task<int> DeleteMultiple(List<int> ids)
    {
        int deleteResult = await DbSet
            .Where(c => ids.Contains(c.Id))
            .ExecuteDeleteAsync();

        return deleteResult;
    }

    public Task<bool> HasAny()
    {
        return DbSet.AnyAsync();
    }

    public virtual async Task<PaginatedList<TEntity>> List(ListingModel listingModel, Func<IQueryable<TEntity>, IQueryable<TEntity>>? queryCallback = null)
    {
        if (listingModel.OrderBy == null) throw new ArgumentException($"The {nameof(ListingModel.OrderBy)} value is required.");
        if (listingModel.Direction == null) throw new ArgumentException($"The {nameof(ListingModel.Direction)} value is required.");
        if (listingModel.Page == null) throw new ArgumentException($"The {nameof(ListingModel.Page)} value is required.");
        if (listingModel.PageSize == null) throw new ArgumentException($"The {nameof(ListingModel.PageSize)} value is required.");

        IQueryable<TEntity> entities = List(DbSet);
        bool desc = listingModel.Direction == ListingModel.DESCENDING;

        entities = sortService.OrderBy(entities, listingModel.OrderBy, desc);
        entities = filterService.FilterBy(entities, listingModel.Filters);
        entities = searchService.GenerateSearchFilters(entities, listingModel.SearchPhrase);

        if (queryCallback != null)
        {
            entities = queryCallback(entities);
        }

        try
        {
            PaginatedList<TEntity> paginatedList = await PaginatedList<TEntity>.CreateAsync(
                entities,
                (int)listingModel.Page,
                (int)listingModel.PageSize);

            return paginatedList;
        }
        catch (Exception e)
        {
            throw new Exception($"An error occured while querying the listing: {e.Message}");
        }
    }
}