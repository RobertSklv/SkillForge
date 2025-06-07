using Microsoft.AspNetCore.Mvc;
using SkillForge.Models.DTOs.Tag;
using SkillForge.Areas.Admin.Services;
using Microsoft.AspNetCore.Authorization;
using SkillForge.Exceptions;

namespace SkillForge.Controllers;

[Authorize(AuthenticationSchemes = "FrontendCookie")]
public class TagController : ApiController
{
    private readonly ITagService service;

    public TagController(ITagService service)
	{
        this.service = service;
    }

    [AllowAnonymous]
    [HttpGet]
    [Route("/Api/Tag/Load/{name}")]
    public async Task<IActionResult> Load([FromRoute] string name)
    {
        TryGetUserId(out int? userId);

        try
        {
            TagPageData pageData = await service.LoadPage(name, userId);

            return Ok(pageData);
        }
        catch (RecordNotFoundException e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpGet]
    public async Task<IActionResult> Search([FromQuery] string? phrase, [FromQuery] List<string>? exclude)
    {
        List<TagLink> tagLinks = await service.SearchLinks(phrase, exclude);

        return Ok(tagLinks);
    }

    [HttpPost]
    public async Task<IActionResult> Follow(TagRequest tagRequest)
    {
        if (!TryGetUserId(out int? userId))
        {
            return Unauthorized();
        }

        try
        {
            await service.Follow((int)userId, tagRequest.Tag);
        }
        catch (RecordNotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (AlreadyFollowingException e)
        {
            return BadRequest(e.Message);
        }

        return Ok();
    }

    [HttpDelete]
    public async Task<IActionResult> Unfollow(TagRequest tagRequest)
    {
        if (!TryGetUserId(out int? userId))
        {
            return Unauthorized();
        }

        try
        {
            await service.Unfollow((int)userId, tagRequest.Tag);
        }
        catch (RecordNotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (AlreadyNotFollowingException e)
        {
            return BadRequest(e.Message);
        }

        return Ok();
    }
}
