using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkillForge.Areas.Admin.Models.DTOs;
using SkillForge.Areas.Admin.Models.DTOs.Article;
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

    public async Task<IActionResult> LoadPage()
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
    public async Task<IActionResult> Latest(int batchIndex, int batchSize = 10)
    {
        List<ArticleCard> cards = await service.GetLatest(batchIndex, batchSize);

        return Ok(cards);
    }
}
