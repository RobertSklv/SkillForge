using SkillForge.Areas.Admin.Models;
using SkillForge.Areas.Admin.Models.Components.Common;
using SkillForge.Areas.Admin.Models.Components.Grid;
using SkillForge.Areas.Admin.Models.DTOs;
using SkillForge.Areas.Admin.Repositories;
using SkillForge.Models.Database;

namespace SkillForge.Areas.Admin.Services;

public abstract class CrudService<TEntity, TViewModel> : ICrudService<TEntity, TViewModel>
    where TEntity : class, IBaseEntity
    where TViewModel : class, IModel
{
    private readonly ICrudRepository<TEntity> repository;

    public virtual string? AreaName { get; } = "Admin";

    public virtual string ControllerName => typeof(TEntity).Name;

    public CrudService(ICrudRepository<TEntity> repository)
    {
        this.repository = repository;
    }

    public virtual TViewModel? InitializeViewModel()
    {
        return Activator.CreateInstance<TViewModel>();
    }

    public virtual async Task<TViewModel?> InitializeViewModelAsync()
    {
        return await Task.FromResult(InitializeViewModel());
    }

    public virtual TViewModel EntityToViewModel(TEntity entity)
    {
        throw new Exception($"Required method {nameof(EntityToViewModel)} was not overridden.");
    }

    public virtual TEntity ViewModelToEntity(TViewModel model)
    {
        throw new Exception($"Required method {nameof(ViewModelToEntity)} was not overridden.");
    }

    public virtual async Task<TViewModel> EntityToViewModelAsync(TEntity entity)
    {
        return await Task.FromResult(EntityToViewModel(entity));
    }

    public virtual async Task<TEntity> ViewModelToEntityAsync(TViewModel model)
    {
        return await Task.FromResult(ViewModelToEntity(model));
    }

    public virtual async Task<bool> Delete(int id)
    {
        return await repository.Delete(id) > 0;
    }

    public virtual async Task<bool> DeleteMultiple(List<int> ids)
    {
        return await repository.DeleteMultiple(ids) > 0;
    }

    public virtual async Task<TEntity?> Get(int id)
    {
        return await repository.Get(id);
    }

    public virtual async Task<TEntity> GetStrict(int id)
    {
        return await repository.GetStrict(id);
    }

    public virtual async Task<List<TEntity>> GetAll()
    {
        return await repository.GetAll();
    }

    public async Task<List<TEntity>> GetByIds(IEnumerable<int> ids)
    {
        return await repository.GetByIds(ids);
    }

    public virtual async Task<PaginatedList<TEntity>> List(ListingModel listingModel)
    {
        return await repository.List(listingModel);
    }

    public virtual async Task<PaginatedList<TEntity>> List(ListingModel<TEntity> listingModel)
    {
        return await repository.List(listingModel);
    }

    public virtual ListingModel<TEntity> InitializeListingModel(ListingModel<TEntity> listingModel, ListingModel listingQuery)
    {
        listingModel.CopyFrom(listingQuery);
        listingModel.AreaName = AreaName;
        listingModel.ControllerName = ControllerName;
        listingModel.ActionName = "Index";

        return listingModel;
    }

    public virtual async Task<Table<TEntity>> CreateListingTable(ListingModel<TEntity> listingModel, PaginatedList<TEntity> items)
    {
        return new Table<TEntity>(listingModel, items)
            .SetSearchable(true)
            .SetOrderable(true)
            .SetFilterable(true)
            .SetAdjustablePageSize(true)
            .AddChainCall(CreateEditRowAction)
            .AddChainCall(CreateDeleteRowAction)
            .AddPagination(true);
    }

    public virtual Table<TEntity> CreateEditRowAction(Table<TEntity> table)
    {
        return table.AddRowAction("Edit", RequestMethod.Get, "bi-pencil-fill", CustomizeEditRowAction);
    }

    public virtual Table<TEntity> CreateDeleteRowAction(Table<TEntity> table)
    {
        return table.AddRowAction("Delete", RequestMethod.Delete, "bi-trash-fill", CustomizeDeleteRowAction);
    }

    public virtual RowAction CustomizeEditRowAction(RowAction action)
    {
        return action;
    }

    public virtual RowAction CustomizeDeleteRowAction(RowAction action)
    {
        return action
            .SetColor(ColorClass.Danger)
            .SetConfirmationTitle("Delete confirmation")
            .SetConfirmationMessage(item => $"Are you sure you want to delete {item.GetType().Name} with ID {item.Id}?");
    }

    public virtual async Task<ListingModel<TEntity>> CreateListingModel(ListingModel listingQuery)
    {
        ListingModel<TEntity> model = new();
        model = InitializeListingModel(model, listingQuery);

        PaginatedList<TEntity> items = await List(model);

        model.Table = await CreateListingTable(model, items);

        return model;
    }

    public virtual async Task<bool> Update(TEntity entity)
    {
        return await repository.Update(entity) > 0;
    }

    public virtual async Task<bool> Upsert(TViewModel model)
    {
        TEntity entity = await ViewModelToEntityAsync(model);
        bool success = await UpsertEntity(entity);
        model.Id = entity.Id;

        return success;
    }

    public virtual async Task<bool> UpsertEntity(TEntity entity)
    {
        return await repository.Upsert(entity) > 0;
    }

    public async Task<bool> UpsertMultiple(List<TEntity> entities)
    {
        return await repository.UpsertMultiple(entities) > 0;
    }

    public Task<bool> HasAny()
    {
        return repository.HasAny();
    }
}

public abstract class CrudService<TEntity> : CrudService<TEntity, TEntity>
    where TEntity : class, IBaseEntity
{
    protected CrudService(ICrudRepository<TEntity> repository)
        : base(repository)
    {
    }

    public sealed override TEntity ViewModelToEntity(TEntity model)
    {
        return model;
    }

    public sealed override TEntity EntityToViewModel(TEntity entity)
    {
        return entity;
    }
}