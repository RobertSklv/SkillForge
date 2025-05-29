using Microsoft.AspNetCore.Mvc;
using SkillForge.Areas.Admin.Models;
using SkillForge.Areas.Admin.Models.Components.Common;
using SkillForge.Areas.Admin.Models.DTOs;
using SkillForge.Areas.Admin.Services;
using SkillForge.Extensions;
using SkillForge.Models.Database;

namespace SkillForge.Areas.Admin.Controllers;

public abstract class CrudController<TEntity, TViewModel> : AdminController
    where TEntity : class, IBaseEntity
    where TViewModel : class, IModel, new()
{
    protected readonly ICrudService<TEntity, TViewModel> _service;

    protected string? ListingTitle { get; set; }

    protected virtual string OldModelTempDataKey => $"OldModel-{typeof(TViewModel).Name}";

    protected virtual string DefaultCreateViewName { get; } = "Upsert";
    protected virtual string DefaultUpdateViewName { get; } = "Upsert";
    protected virtual bool AddCreateActionOnIndexPage { get; } = true;

    protected virtual Func<ListingModel, dynamic> DefaultListingMethod => _service.CreateListingModel;

    public CrudController(ICrudService<TEntity, TViewModel> service)
    {
        _service = service;
    }

    public virtual async Task UpsertMethod(TViewModel model) => await _service.Upsert(model);

    protected virtual Task<string> MassAction(string massAction, List<int> selectedItemIds) => Task.FromResult("Index");

    [HttpGet]
    public virtual async Task<IActionResult> Index([FromQuery] ListingModel listingModel)
    {
        if (AddCreateActionOnIndexPage)
        {
            AddCreateAction();
        }

        if (ListingTitle != null)
        {
            ViewData["Title"] = ListingTitle;
            ViewData["PageHeading"] = ListingTitle;
        }

        return View(await DefaultListingMethod(listingModel));
    }

    [HttpGet]
    public virtual async Task<IActionResult> View(int id)
    {
        TViewModel? viewModel = await GetViewModel(id);

        return await ViewWithBackAction(viewModel);
    }

    [NonAction]
    public virtual async Task<IActionResult> ViewWithBackAction(TViewModel? viewModel)
    {
        if (viewModel != null)
        {
            AddBackAction();

            return base.View(viewModel);
        }

        Alert($"Entity {typeof(TEntity).Name} doesn't exist.", ColorClass.Danger);

        return RedirectToAction("Index");
    }

    [HttpGet]
    public virtual async Task<IActionResult> Create()
    {
        AddBackAction();

        TViewModel? model = TempData.Pop<TViewModel>(OldModelTempDataKey);

        if (model == null)
        {
            model = await _service.InitializeViewModelAsync();
        }

        return View(DefaultCreateViewName, model);
    }

    [HttpGet]
    public virtual async Task<IActionResult> Edit(int id)
    {
        TViewModel? viewModel;
        if (TempData.ContainsKey(OldModelTempDataKey))
        {
            viewModel = TempData.Pop<TViewModel>(OldModelTempDataKey);
        }
        else
        {
            viewModel = await GetViewModel(id);
        }

        return await Edit(viewModel);
    }

    [NonAction]
    public virtual async Task<IActionResult> Edit(TViewModel? viewModel)
    {
        if (viewModel != null)
        {
            AddBackAction();

            return base.View(DefaultUpdateViewName, viewModel);
        }

        Alert($"Entity {typeof(TEntity).Name} doesn't exist.", ColorClass.Danger);

        return RedirectToAction("Index");
    }

    [HttpPost]
    public virtual async Task<IActionResult> Create(TViewModel model)
    {
        if (!ModelState.IsValid)
        {
            string errors = string.Join(", ", GetModelStateErrors());
            Alert("The provided data is not valid, please resolve all validation errors before submitting the form. Errors: " + errors, ColorClass.Danger);

            TempData.Set(OldModelTempDataKey, model);

            return RedirectToAction("Create");
        }

        try
        {
            await UpsertMethod(model);
        }
        catch (Exception e)
        {
            Alert($"An error occured: {e.Message}", ColorClass.Danger);

            TempData.Set(OldModelTempDataKey, model);

            return RedirectToAction("Create");
        }

        Alert("Record successfully created.", ColorClass.Success);

        return RedirectToAction("Index");
    }

    [HttpPost]
    public virtual async Task<IActionResult> Update(TViewModel model)
    {
        if (!ModelState.IsValid)
        {
            string errors = string.Join(", ", GetModelStateErrors());
            Alert("The provided data is not valid, please resolve all validation errors before submitting the form. Errors: " + errors, ColorClass.Danger);

            TempData.Set(OldModelTempDataKey, model);

            return RedirectToAction("Edit", new { model.Id });
        }

        try
        {
            await UpsertMethod(model);
        }
        catch (Exception e)
        {
            Alert($"An error occured: {e.Message}", ColorClass.Danger);

            TempData.Set(OldModelTempDataKey, model);

            return RedirectToAction("Edit", new { model.Id });
        }

        Alert("Record successfully saved.", ColorClass.Success);

        return RedirectToAction("Edit", new { model.Id });
    }

    public virtual async Task<IActionResult> Delete(int id)
    {
        TEntity? entity = await GetEntity(id);

        if (entity != null)
        {
            try
            {
                await _service.Delete(id);
                Alert("Record successfully deleted.", ColorClass.Success);
            }
            catch (Exception e)
            {
                Alert($"An error occured: {e.Message}", ColorClass.Danger);
            }
        }
        else
        {
            Alert($"Entity {typeof(TEntity).Name} with ID {id} doesn't exist.", ColorClass.Danger);
        }

        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> Mass(List<int> selectedItemIds, [FromQuery] string massAction, [FromQuery] ListingModel? listingModel = null)
    {
        string backAction = await MassAction(massAction, selectedItemIds);

        return RedirectToAction(backAction, listingModel?.GenerateListingQuery());
    }

    protected virtual async Task<TViewModel?> GetViewModel(int id)
    {
        TEntity? entity = await GetEntity(id);
        if (entity == null)
        {
            Alert($"Entity {typeof(TEntity).Name} with ID {id} doesn't exist.", ColorClass.Danger);

            return null;
        }

        TViewModel? model = await _service.EntityToViewModelAsync(entity);

        return model;
    }

    protected virtual async Task<TEntity?> GetEntity(int id)
    {
        return await _service.Get(id);
    }
}

public abstract class CrudController<TEntity> : CrudController<TEntity, TEntity>
    where TEntity : class, IBaseEntity, new()
{
    protected CrudController(
        ICrudService<TEntity> service)
        : base(service)
    {
    }
}