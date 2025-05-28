using Microsoft.AspNetCore.Mvc;
using SkillForge.Areas.Admin.Services;

namespace SkillForge.Controllers;

public class ImageController : ApiController
{
    private readonly IImageService service;

    public ImageController(IImageService service)
    {
        this.service = service;
    }

    [HttpPost]
    public async Task<IActionResult> Upload([FromForm] IFormFile image)
    {
        if (!service.ValidateImageSize(image))
        {
            return ValidationProblem("The uploaded image is too big. Please upload an image not bigger than 128 MB.");
        }

        string filename = await service.UploadImageAsync(image, "articles/uploads");

        return Ok(filename);
    }
}
