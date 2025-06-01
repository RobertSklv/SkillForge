using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkillForge.Areas.Admin.Models.DTOs.Comment;
using SkillForge.Areas.Admin.Models.DTOs.Rating;
using SkillForge.Areas.Admin.Services;

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
    public async Task<IActionResult> Add(CommentAddDTO form)
    {
        await userService.GetStrict(UserId);

        await service.Add(UserId, form.ArticleId, form.Content);

        return Ok();
    }

    [HttpPost]
    [Route("/Api/Comment/Rate/{id}")]
    public async Task<IActionResult> Rate([FromRoute] int id, [FromBody] UserRatingData rate)
    {
        await service.Rate(UserId, id, rate);

        return Ok();
    }
}
