using Microsoft.AspNetCore.Mvc;
using SkillForge.Areas.Admin.Models.DTOs;

namespace SkillForge.Controllers;

public class ArticleController : ApiController
{
    [HttpPost]
    public async Task<IActionResult> Create([FromForm] ArticleCreateDTO form)
    {
        return Ok();
    }
}
