using Microsoft.AspNetCore.Mvc;
using SkillForge.Areas.Admin.Services;

namespace SkillForge.Controllers;

public class ImageController : ApiController
{
    public static readonly Dictionary<string, string> ImageRoutes = new()
    {
        { "article", "articles/uploads" },
        { "comment", "comments/uploads" },
    };

    private readonly IImageService service;

    public ImageController(IImageService service)
    {
        this.service = service;
    }

    [HttpPost]
    public async Task<IActionResult> Upload([FromForm] IFormFile image, [FromQuery] string type)
    {
        if (!service.ValidateImageSize(image))
        {
            return ValidationProblem("The uploaded image is too big. Please upload an image not bigger than 128 MB.");
        }

        if (!ImageRoutes.ContainsKey(type))
        {
            return BadRequest($"Invalid image type: {type}");
        }

        string directory = ImageRoutes[type];
        string filename = await service.UploadImageAsync(image, directory);

        return Ok(filename);
    }
}
