using Microsoft.AspNetCore.Mvc;

namespace SkillForge.Controllers;

[ApiController]
[Area("Api")]
[Route("/Api/[controller]/[action]")]
public abstract class ApiController : Controller
{
}