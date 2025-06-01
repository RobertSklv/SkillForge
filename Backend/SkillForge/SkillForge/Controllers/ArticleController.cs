using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkillForge.Areas.Admin.Models.DTOs;
using SkillForge.Areas.Admin.Models.DTOs.Article;
using SkillForge.Areas.Admin.Models.DTOs.Rating;
using SkillForge.Areas.Admin.Services;
using SkillForge.Models.Database;

namespace SkillForge.Controllers;

[Authorize(AuthenticationSchemes = "FrontendCookie")]
public class ArticleController : ApiController
{
    private readonly IArticleService service;
    private readonly ICategoryService categoryService;

    public ArticleController(IArticleService service, ICategoryService categoryService)
    {
        this.service = service;
        this.categoryService = categoryService;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromForm] ArticleCreateDTO form)
    {
        if (!TryGetUserId(out int? userId))
        {
            throw new Exception("Server error.");
        }

        await service.UserCreate(form, (int)userId);

        return Ok("Article created successfully.");
    }

    [HttpGet]
    public async Task<IActionResult> LoadCreatePage()
    {
        List<Category> categories = await categoryService.GetAll();

        ArticleCreatePageModel pageModel = new()
        {
            CategoryOptions = categories.ConvertAll(c => new EntityOption()
            {
                Value = c.Id,
                Label = c.DisplayedName
            })
        };

        return Ok(pageModel);
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> Latest(int batchIndex, int batchSize = 10)
    {
        List<ArticleCard> cards;

        if (TryGetUserId(out int? _))
        {
            cards = await service.GetLatest(UserId, batchIndex, batchSize);
        }
        else
        {
            cards = await service.GetLatest(batchIndex, batchSize);
        }

        return Ok(cards);
    }

    [AllowAnonymous]
    [HttpGet]
    [Route("/Api/Article/View/{id}")]
    public async Task<IActionResult> View(int id)
    {
        ArticlePageModel model;

        if (TryGetUserId(out int? _))
        {
            model = await service.View(UserId, id);
        }
        else
        {
            model = await service.View(GuestId, id);
        }

        return Ok(model);
    }

    [HttpPost]
    [Route("/Api/Article/Rate/{id}")]
    public async Task<IActionResult> Rate([FromRoute] int id, [FromBody] UserRatingData rate)
    {
        await service.Rate(UserId, id, rate);

        return Ok();
    }
}
