using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkillForge.Areas.Admin.Models.DTOs;
using SkillForge.Areas.Admin.Models.DTOs.Article;
using SkillForge.Areas.Admin.Models.DTOs.Comment;
using SkillForge.Areas.Admin.Services;
using SkillForge.Models.Database;

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
    //[Route("/Api/Comment/Add/{articleId}")]
    public async Task<IActionResult> Add(CommentAddDTO form)
    {
        await userService.GetStrict(UserId);

        await service.Add(UserId, form.ArticleId, form.Content);

        return Ok();
    }
}
