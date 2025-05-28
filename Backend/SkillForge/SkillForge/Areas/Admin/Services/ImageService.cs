namespace SkillForge.Areas.Admin.Services;

public class ImageService : IImageService
{
    public const long MAXIMUM_IMAGE_SIZE = 134217728;

    private readonly IWebHostEnvironment webHostEnvironment;

    public ImageService(IWebHostEnvironment webHostEnvironment)
    {
        this.webHostEnvironment = webHostEnvironment;
    }

    public void RemoveImage(string directory, string filename)
    {
        try
        {
            string path = Path.Combine(GetImagesPath(), directory, filename);
            File.Delete(path);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<string> UploadImageAsync(IFormFile image, string directory)
    {
        string extension = Path.GetExtension(image.FileName);
        string filename = $"{DateTime.Now:yyyyMMddhhmmssfff}{extension}";
        string path = Path.Combine(GetImagesPath(), directory, filename);

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

    public bool ValidateImageSize(IFormFile file)
    {
        return file.Length <= MAXIMUM_IMAGE_SIZE;
    }

    public string GetImagesPath()
    {
        string wwwRootPath = webHostEnvironment.WebRootPath;
        string imagesPath = Path.Combine(wwwRootPath, @"images\");

        return imagesPath;
    }
}
