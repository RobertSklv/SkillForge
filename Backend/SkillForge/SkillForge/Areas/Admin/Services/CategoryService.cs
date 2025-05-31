using SkillForge.Areas.Admin.Models;
using SkillForge.Areas.Admin.Models.Components.Grid;
using SkillForge.Areas.Admin.Models.DTOs;
using SkillForge.Areas.Admin.Repositories;
using SkillForge.Models.Database;

namespace SkillForge.Areas.Admin.Services;

public class CategoryService : CrudService<Category, CategoryVM>, ICategoryService
{
    private readonly ICategoryRepository repository;
    private readonly IImageService imageService;

    public CategoryService(ICategoryRepository repository, IImageService imageService)
        : base(repository)
    {
        this.repository = repository;
        this.imageService = imageService;
    }

    public override async Task<Table<Category>> CreateListingTable(ListingModel<Category> listingModel, PaginatedList<Category> items)
    {
        return (await base.CreateListingTable(listingModel, items))
            .SetSelectableOptionsSource(nameof(Category.Parent), await GetAll());
    }

    public override Category ViewModelToEntity(CategoryVM model)
    {
        return new Category
        {
            Id = model.Id,
            ParentId = model.ParentId,
            Code = model.Code,
            DisplayedName = model.DisplayedName,
            Description = model.Description,
            Image = model.CurrentImageFilename,
        };
    }

    public override CategoryVM EntityToViewModel(Category entity)
    {
        return new CategoryVM
        {
            Id = entity.Id,
            ParentId = entity.ParentId,
            Code = entity.Code,
            DisplayedName = entity.DisplayedName,
            Description = entity.Description,
            CurrentImageFilename = entity.Image,
        };
    }

    public override async Task<bool> Upsert(CategoryVM model)
    {
        Category entity = ViewModelToEntity(model);

        if (entity.ParentId == 0)
        {
            entity.ParentId = null;
        }

        if (model.Image != null)
        {
            if (model.CurrentImageFilename != null)
            {
                imageService.RemoveImage("categories", model.CurrentImageFilename);
            }

            entity.Image = await UploadImageAsync(model.Image);
        }
        else if (model.RemoveImage && model.CurrentImageFilename != null)
        {
            imageService.RemoveImage("categories", model.CurrentImageFilename);

            entity.Image = null;
        }

        return await UpsertEntity(entity);
    }

    public async Task<string> UploadImageAsync(IFormFile image)
    {
        return await imageService.UploadImageAsync(image, "categories");
    }

    public Task<List<Category>> GetPossibleParents(int id)
    {
        return repository.GetPossibleParents(id);
    }

    public Task<List<Category>> GetPossibleParents()
    {
        return repository.GetPossibleParents();
    }
}
