using Microsoft.AspNetCore.Mvc;
using SkillForge.Areas.Admin.Models.Components.Pages;
using SkillForge.Areas.Admin.Models.DTOs;
using SkillForge.Areas.Admin.Services;
using SkillForge.Attributes;
using SkillForge.Models.Database;

namespace SkillForge.Areas.Admin.Controllers;

[AdminNavMenu("tags", "Tags", SortOrder = 40)]
[AdminNavLink("All Tags", "Index", Menu = "tags")]
public class TagController : CrudController<Tag>
{
    public const string GENERAL_LINK = "general";
    public const string ARTICLES_LINK = "articles";

    private readonly ITagService service;
    private readonly IArticleService articleService;

    public TagController(ITagService service, IArticleService articleService)
        : base(service)
    {
        this.service = service;
        this.articleService = articleService;
    }

    public override Task<IActionResult> Edit(int id)
    {
        GenerateSidebarLinks(id, GENERAL_LINK);

        return base.Edit(id);
    }

    [Route("/Admin/Tag/{id}/Articles")]
    public async Task<IActionResult> Articles([FromQuery] ListingModel listingQuery, [FromRoute] int id)
    {
        AddBackAction();
        GenerateSidebarLinks(id, ARTICLES_LINK);

        return View(await articleService.CreateListingByTag(listingQuery, id));
    }

    public void GenerateSidebarLinks(int id, string activeLink)
    {
        SidebarLinkGroup sidebar = GetOrCreateSidebarLinkGroup();
        sidebar.ActiveLinkId = activeLink;

        sidebar.Add(new SidebarLink
        {
            Id = GENERAL_LINK,
            Content = "General",
            Route = $"/Admin/Tag/Edit/{id}",
        });

        sidebar.Add(new SidebarLink
        {
            Id = ARTICLES_LINK,
            Content = "Articles",
            Route = $"/Admin/Tag/{id}/Articles",
        });
    }
}