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

    public override Category ViewModelToEntity(CategoryVM model)
    {
        return new Category
        {
            Id = model.Id,
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
            Code = entity.Code,
            DisplayedName = entity.DisplayedName,
            Description = entity.Description,
            CurrentImageFilename = entity.Image,
        };
    }

    public override async Task<bool> Upsert(CategoryVM model)
    {
        Category entity = ViewModelToEntity(model);

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
}
