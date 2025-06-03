using Microsoft.AspNetCore.Mvc;
using SkillForge.Models.DTOs.Tag;
using SkillForge.Areas.Admin.Services;

namespace SkillForge.Controllers;

public class TagController : ApiController
{
    private readonly ITagService service;

    public TagController(ITagService service)
	{
        this.service = service;
    }

    [HttpGet]
    public async Task<IActionResult> Search([FromQuery] string? phrase, [FromQuery] List<string>? exclude)
    {
        List<TagLink> tagLinks = await service.SearchLinks(phrase, exclude);

        return Ok(tagLinks);
    }
}
