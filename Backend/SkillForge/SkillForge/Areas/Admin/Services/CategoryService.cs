using SkillForge.Areas.Admin.Models.DTOs;
using SkillForge.Areas.Admin.Repositories;
using SkillForge.Models.Database;

namespace SkillForge.Areas.Admin.Services;

public class CategoryService : CrudService<Category, CategoryVM>, ICategoryService
{
    private readonly ICategoryRepository repository;
    private readonly IWebHostEnvironment webHostEnvironment;

    public CategoryService(ICategoryRepository repository, IWebHostEnvironment webHostEnvironment)
        : base(repository)
    {
        this.repository = repository;
        this.webHostEnvironment = webHostEnvironment;
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
                RemoveImage(model.CurrentImageFilename);
            }

            entity.Image = await UploadImageAsync(model.Image);
        }
        else if (model.RemoveImage && model.CurrentImageFilename != null)
        {
            RemoveImage(model.CurrentImageFilename);

            entity.Image = null;
        }

        return await UpsertEntity(entity);
    }

    public string GetImagesPath()
    {
        string wwwRootPath = webHostEnvironment.WebRootPath;
        string imagesPath = Path.Combine(wwwRootPath, @"images\categories");

        return imagesPath;
    }

    public void RemoveImage(string filename)
    {
        try
        {
            string path = Path.Combine(GetImagesPath(), filename);
            File.Delete(path);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<string> UploadImageAsync(IFormFile image, string imagesPath)
    {
        string extension = Path.GetExtension(image.FileName);
        string filename = $"{DateTime.Now:yyyyMMddhhmmssfff}{extension}";
        string path = Path.Combine(imagesPath, filename);

        try
        {
            using FileStream stream = new(path, FileMode.Create);
            await image.CopyToAsync(stream);
        }
        catch (Exception)
        {
            throw;
        }

        return filename;
    }

    public async Task<string> UploadImageAsync(IFormFile image)
    {
        string imagesPath = GetImagesPath();

        return await UploadImageAsync(image, imagesPath);
    }
}
