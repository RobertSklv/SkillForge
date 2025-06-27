using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkillForge.Areas.Admin.Services;
using SkillForge.Exceptions;
using SkillForge.Models.Database;
using SkillForge.Models.DTOs.Article;
using SkillForge.Models.DTOs.Comment;
using SkillForge.Models.DTOs.Rating;
using SkillForge.Models.DTOs.User;

namespace SkillForge.Controllers;

[Authorize(AuthenticationSchemes = "FrontendCookie")]
public class CommentController : ApiController
{
    private readonly ICommentService service;
    private readonly IUserService userService;

    public CommentController(ICommentService service, IUserService userService)
    {
        this.service = service;
        this.userService = userService;
    }

    [HttpPost]
    public async Task<IActionResult> Upsert(CommentUpsertFormData form)
    {
        try
        {
            CommentModel comment = await service.Upsert(UserId, form);

            return Ok(comment);
        }
        catch (NotOwnedByUserException)
        {
            return Unauthorized();
        }
    }

    [HttpDelete]
    [Route("/Api/Comment/Delete/{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        Comment comment = await service.GetStrict(id);

        if (comment.UserId != UserId)
        {
            return Unauthorized();
        }

        await service.Delete(id);

        return Ok("Comment deleted successfully");
    }

    [HttpPost]
    [Route("/Api/Comment/Rate/{id}")]
    public async Task<IActionResult> Rate([FromRoute] int id, [FromBody] UserRatingData rate)
    {
        await service.Rate(UserId, id, rate);

        return Ok();
    }

    [Authorize(AuthenticationSchemes = "FrontendCookie")]
    [AllowAnonymous]
    [HttpGet]
    [Route("/Api/Comment/PositiveRates/{id}")]
    public async Task<IActionResult> PositiveRates([FromRoute] int id, [FromQuery] int batchIndex, [FromQuery] int batchSize)
    {
        TryGetUserId(out int? userId);

        if (userId != null && await userService.IsSuspended((int)userId))
        {
            return BadRequest("Your account is temporarily suspended");
        }

        List<UserListItem> items = await service.GetRating(id, userId, batchIndex, batchSize, positive: true);

        return Ok(items);
    }

    [Authorize(AuthenticationSchemes = "FrontendCookie")]
    [AllowAnonymous]
    [HttpGet]
    [Route("/Api/Comment/NegativeRates/{id}")]
    public async Task<IActionResult> NegativeRates([FromRoute] int id, [FromQuery] int batchIndex, [FromQuery] int batchSize)
    {
        TryGetUserId(out int? userId);

        if (userId != null && await userService.IsSuspended((int)userId))
        {
            return BadRequest("Your account is temporarily suspended");
        }

        List<UserListItem> items = await service.GetRating(id, userId, batchIndex, batchSize, positive: false);

        return Ok(items);
    }
}
