namespace SkillForge.Areas.Admin.Services;

public interface IImageService
{
    void RemoveImage(string directory, string filename);

    Task<string> UploadImageAsync(IFormFile image, string directory);

    bool ValidateImageSize(IFormFile file);

    string GetImagesPath();
}