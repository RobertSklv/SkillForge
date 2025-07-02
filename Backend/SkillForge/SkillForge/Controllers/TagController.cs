using Microsoft.AspNetCore.Mvc;
using SkillForge.Models.DTOs.Tag;
using SkillForge.Areas.Admin.Services;
using Microsoft.AspNetCore.Authorization;
using SkillForge.Exceptions;
using SkillForge.Models.DTOs.User;
using SkillForge.Services;
using SkillForge.Models.Database;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace SkillForge.Controllers;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class TagController : ApiController
{
    private readonly ITagService service;
    private readonly IUserFeedService userFeedService;

    public TagController(ITagService service, IUserFeedService userFeedService)
	{
        this.service = service;
        this.userFeedService = userFeedService;
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

    [AllowAnonymous]
    [HttpGet]
    [Route("/Api/Tag/Followers/{tag}")]
    public async Task<IActionResult> Followers([FromRoute] string tag, [FromQuery] int batchIndex, [FromQuery] int batchSize)
    {
        Tag? t = await service.GetByName(tag);

        if (t == null) return NotFound("Tag not found");

        TryGetUserId(out int? userId);

        List<UserListItem> followers = await userFeedService.GetTagFollowers(t.Id, userId, batchIndex, batchSize);

        return Ok(followers);
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
