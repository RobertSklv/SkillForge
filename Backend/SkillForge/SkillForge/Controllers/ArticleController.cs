using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkillForge.Models.DTOs.Article;
using SkillForge.Models.DTOs.Rating;
using SkillForge.Areas.Admin.Services;
using SkillForge.Services;
using SkillForge.Models.DTOs.Search;
using SkillForge.Exceptions;
using SkillForge.Models.DTOs.User;

namespace SkillForge.Controllers;

public class ArticleController : ApiController
{
    private readonly IArticleService service;
    private readonly IUserFeedService userFeedService;
    private readonly IUserService userService;

    public ArticleController(
        IArticleService service,
        IUserFeedService userFeedService,
        IUserService userService)
    {
        this.service = service;
        this.userFeedService = userFeedService;
        this.userService = userService;
    }

    [Authorize(AuthenticationSchemes = "FrontendCookie")]
    [HttpPost]
    public async Task<IActionResult> Upsert(ArticleUpsertDTO form)
    {
        if (!TryGetUserId(out int? userId))
        {
            throw new Exception("Server error.");
        }

        if (await userService.IsSuspended((int)userId))
        {
            return BadRequest("Your account is temporarily suspended");
        }

        if (!service.ValidateTagNames(form.Tags, nameof(form.Tags), ModelState))
        {
            return ValidationProblem(ModelState);
        }

        try
        {
            await service.UserUpsert(form, (int)userId);
        }
        catch (NotOwnedByUserException e)
        {
            return Unauthorized(e.Message);
        }
        catch (RecordDeletedException)
        {
            return NotFound();
        }

        if (form.Id == 0)
        {
            return Ok("Article created successfully.");
        }
        else
        {
            return Ok("Article edited successfully.");
        }
    }

    [Authorize(AuthenticationSchemes = "FrontendCookie")]
    [HttpGet]
    public async Task<IActionResult> LoadUpsertPage(int? id)
    {
        try
        {
            ArticleUpsertPageModel pageModel = await service.LoadUpsertPage(id, UserId);

            return Ok(pageModel);
        }
        catch (NotOwnedByUserException e)
        {
            return Unauthorized(e.Message);
        }
        catch (RecordDeletedException)
        {
            return NotFound();
        }
    }

    [Authorize(AuthenticationSchemes = "FrontendCookie")]
    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> Latest(int batchIndex, int batchSize = 10)
    {
        List<ArticleCard> cards;

        TryGetUserId(out int? userId);

        if (userId != null && await userService.IsSuspended((int)userId))
        {
            return BadRequest("Your account is temporarily suspended");
        }

        cards = await userFeedService.GetLatestArticles(userId, batchIndex, batchSize);

        return Ok(cards);
    }

    [Authorize(AuthenticationSchemes = "FrontendCookie")]
    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> LatestByTag(string tag, int batchIndex, int batchSize = 10)
    {
        List<ArticleCard> cards;

        TryGetUserId(out int? userId);

        if (userId != null && await userService.IsSuspended((int)userId))
        {
            return BadRequest("Your account is temporarily suspended");
        }

        cards = await userFeedService.GetLatestArticlesByTag(tag, userId, batchIndex, batchSize);

        return Ok(cards);
    }

    [Authorize(AuthenticationSchemes = "FrontendCookie")]
    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> LatestByAuthor(string authorName, int batchIndex, int batchSize = 10)
    {
        List<ArticleCard> cards;

        TryGetUserId(out int? userId);

        if (userId != null && await userService.IsSuspended((int)userId))
        {
            return BadRequest("Your account is temporarily suspended");
        }

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
    public async Task<IActionResult> SearchAdvanced([FromQuery] GridState gridState)
    {
        PaginationResponse<ArticleCard> res = await service.SearchAdvancedCards(gridState);

        return Ok(res);
    }

    [Authorize(AuthenticationSchemes = "FrontendCookie")]
    [AllowAnonymous]
    [HttpGet]
    [Route("/Api/Article/View/{id}")]
    public async Task<IActionResult> View(int id)
    {
        ArticlePageData model;

        try
        {
            if (TryGetUserId(out int? userId))
            {
                if (await userService.IsSuspended((int)userId))
                {
                    return BadRequest("Your account is temporarily suspended");
                }

                model = await service.View((int)userId, id);
            }
            else
            {
                model = await service.View(GuestId, id);
            }
        }
        catch (RecordDeletedException)
        {
            return NotFound();
        }

        return Ok(model);
    }

    [Authorize(AuthenticationSchemes = "FrontendCookie")]
    [HttpPost]
    [Route("/Api/Article/Rate/{id}")]
    public async Task<IActionResult> Rate([FromRoute] int id, [FromBody] UserRatingData rate)
    {
        if (await userService.IsSuspended(UserId))
        {
            return BadRequest("Your account is temporarily suspended");
        }

        await service.Rate(UserId, id, rate);

        return Ok();
    }

    [Authorize(AuthenticationSchemes = "FrontendCookie")]
    [AllowAnonymous]
    [HttpGet]
    [Route("/Api/Article/PositiveRates/{id}")]
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
    [Route("/Api/Article/NegativeRates/{id}")]
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
