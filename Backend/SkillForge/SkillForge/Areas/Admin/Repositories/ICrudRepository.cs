using Microsoft.EntityFrameworkCore;
using SkillForge.Areas.Admin.Models;
using SkillForge.Areas.Admin.Models.DTOs;
using SkillForge.Models.Database;

namespace SkillForge.Areas.Admin.Repositories;

public interface ICrudRepository<TEntity>
    where TEntity : class, IBaseEntity
{
    abstract DbSet<TEntity> DbSet { get; }

    Task<TEntity?> Get(int id);

    Task<TEntity> GetStrict(int id);

    Task<List<TEntity>> GetAll();

    Task<List<TEntity>> GetByIds(IEnumerable<int> ids);

    Task<int> Upsert(TEntity entity);

    Task<int> Update(TEntity entity);

    Task<int> SaveMultiple(List<TEntity> entities);

    Task<int> Delete(int id);

    Task<int> DeleteMultiple(List<int> ids);

    Task<PaginatedList<TEntity>> List(ListingModel listingModel, Func<IQueryable<TEntity>, IQueryable<TEntity>>? queryCallback = null);

    Task<bool> HasAny();
}