using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc;

namespace SkillForge.Controllers;

[ApiController]
[Area("Api")]
[Route("/Api/[controller]/[action]")]
public abstract class ApiController : Controller
{
    public int UserId => int.Parse(User.Claims.Where(c => c.Type == "Id").First().Value);

	public string? GuestId => HttpContext.Request.Cookies["guest_id"]!;

    public bool TryGetUserId([NotNullWhen(true)] out int? userId)
    {
        userId = null;

        try
		{
			userId = UserId;
		}
		catch (Exception)
		{
			return false;
		}

		return true;
    }
}