using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkillForge.Areas.Admin.Models.DTOs;
using SkillForge.Models.DTOs.Article;
using SkillForge.Models.DTOs.Rating;
using SkillForge.Areas.Admin.Services;
using SkillForge.Models.Database;
using SkillForge.Services;

namespace SkillForge.Controllers;

[Authorize(AuthenticationSchemes = "FrontendCookie")]
public class ArticleController : ApiController
{
    private readonly IArticleService service;
    private readonly ICategoryService categoryService;
    private readonly IUserFeedService userFeedService;

    public ArticleController(IArticleService service, ICategoryService categoryService, IUserFeedService userFeedService)
    {
        this.service = service;
        this.categoryService = categoryService;
        this.userFeedService = userFeedService;
    }

    [HttpPost]
    public async Task<IActionResult> Upsert(ArticleUpsertDTO form)
    {
        if (!TryGetUserId(out int? userId))
        {
            throw new Exception("Server error.");
        }

        if (!service.ValidateTagNames(form.Tags, nameof(form.Tags), ModelState))
        {
            return ValidationProblem(ModelState);
        }

        await service.UserUpsert(form, (int)userId);

        if (form.Id == 0)
        {
            return Ok("Article created successfully.");
        }
        else
        {
            return Ok("Article edited successfully.");
        }
    }

    [HttpGet]
    public async Task<IActionResult> LoadUpsertPage(int? id)
    {
        List<Category> categories = await categoryService.GetAll();

        ArticleUpsertPageModel pageModel = new()
        {
            CategoryOptions = categories.ConvertAll(c => new EntityOption()
            {
                Value = c.Id,
                Label = c.DisplayedName
            })
        };

        if (id != null)
        {
            Article article = await service.GetStrict((int)id);

            if (article.AuthorId != UserId)
            {
                return Unauthorized();
            }

            pageModel.CurrentState = new ArticleState
            {
                Model = new ArticleUpsertDTO
                {
                    Id = article.Id,
                    Image = article.Image,
                    Title = article.Title,
                    Content = article.Content,
                    CategoryId = article.CategoryId,
                    Tags = article.Tags!.ConvertAll(t => t.Tag!.Name).ToList(),
                },
                IsApproved = article.ApprovalId != null
            };
        }

        return Ok(pageModel);
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> Latest(int batchIndex, int batchSize = 10)
    {
        List<ArticleCard> cards;

        TryGetUserId(out int? userId);

        cards = await userFeedService.GetLatestArticles(userId, batchIndex, batchSize);

        return Ok(cards);
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> LatestByTag(string tag, int batchIndex, int batchSize = 10)
    {
        List<ArticleCard> cards;

        TryGetUserId(out int? userId);

        cards = await userFeedService.GetLatestArticlesByTag(tag, userId, batchIndex, batchSize);

        return Ok(cards);
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> LatestByAuthor(string authorName, int batchIndex, int batchSize = 10)
    {
        List<ArticleCard> cards;

        TryGetUserId(out int? userId);

        cards = await userFeedService.GetLatestArticlesByAuthor(authorName, userId, batchIndex, batchSize);

        return Ok(cards);
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> Search(string p)
    {
        List<ArticleSearchItem> items = await service.SearchItems(p);

        return Ok(items);
    }

    [AllowAnonymous]
    [HttpGet]
    [Route("/Api/Article/View/{id}")]
    public async Task<IActionResult> View(int id)
    {
        ArticlePageData model;

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
