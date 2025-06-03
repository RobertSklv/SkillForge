using Microsoft.AspNetCore.Mvc;
using SkillForge.Areas.Admin.Services;
using SkillForge.Models.DTOs.Home;

namespace SkillForge.Controllers;

public class HomeController : ApiController
{
    private readonly IHomeService service;

    public HomeController(IHomeService service)
	{
        this.service = service;
    }

    public async Task<IActionResult> Load()
    {
        HomePageData pageData = await service.LoadPage();

        return Ok(pageData);
    }
}
