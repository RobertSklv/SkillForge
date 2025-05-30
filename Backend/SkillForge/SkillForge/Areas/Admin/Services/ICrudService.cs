using SkillForge.Areas.Admin.Models;
using SkillForge.Areas.Admin.Models.Components.Grid;
using SkillForge.Areas.Admin.Models.DTOs;
using SkillForge.Models.Database;

namespace SkillForge.Areas.Admin.Services;

public interface ICrudService<TEntity, TViewModel>
    where TEntity : class, IBaseEntity
    where TViewModel : class, IModel
{
    string? AreaName { get; }

    string ControllerName { get; }

    Task<TEntity?> Get(int id);

    Task<TEntity> GetStrict(int id);

    Task<List<TEntity>> GetAll();

    Task<List<TEntity>> GetByIds(IEnumerable<int> ids);

    Task<bool> Upsert(TViewModel model);

    Task<bool> UpsertEntity(TEntity entity);

    Task<bool> Update(TEntity entity);

    Task<bool> UpsertMultiple(List<TEntity> entities);

    Task<bool> Delete(int id);

    Task<bool> DeleteMultiple(List<int> ids);

    Task<PaginatedList<TEntity>> List(ListingModel listingModel);

    TViewModel? InitializeViewModel();

    Task<TViewModel?> InitializeViewModelAsync();

    TViewModel EntityToViewModel(TEntity entity);

    TEntity ViewModelToEntity(TViewModel model);

    Task<TViewModel> EntityToViewModelAsync(TEntity entity);

    Task<TEntity> ViewModelToEntityAsync(TViewModel model);

    Task<PaginatedList<TEntity>> List(ListingModel<TEntity> listingModel);

    ListingModel<TEntity> InitializeListingModel(ListingModel<TEntity> listingModel, ListingModel listingQuery);

    Task<Table<TEntity>> CreateListingTable(ListingModel<TEntity> listingModel, PaginatedList<TEntity> items);

    Task<ListingModel<TEntity>> CreateListingModel(ListingModel listingQuery);

    Task<bool> HasAny();
}

public interface ICrudService<TEntity> : ICrudService<TEntity, TEntity>
    where TEntity : class, IBaseEntity
{
}