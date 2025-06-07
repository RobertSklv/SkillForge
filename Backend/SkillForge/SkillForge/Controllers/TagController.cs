using Microsoft.AspNetCore.Mvc;
using SkillForge.Models.DTOs.Tag;
using SkillForge.Areas.Admin.Services;
using SkillForge.Models.Database;

namespace SkillForge.Controllers;

public class TagController : ApiController
{
    private readonly ITagService service;

    public TagController(ITagService service)
	{
        this.service = service;
    }

    [HttpGet]
    [Route("/Api/Tag/Load/{name}")]
    public async Task<IActionResult> Load([FromRoute] string name)
    {
        Tag? tag = await service.GetByName(name);

        if (tag == null) return NotFound();

        TryGetUserId(out int? userId);

        TagPageData pageData = await service.LoadPage(tag, userId);

        return Ok(pageData);
    }

    [HttpGet]
    public async Task<IActionResult> Search([FromQuery] string? phrase, [FromQuery] List<string>? exclude)
    {
        List<TagLink> tagLinks = await service.SearchLinks(phrase, exclude);

        return Ok(tagLinks);
    }
}
