using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace SkillForge.Areas.Admin.Models.ViewComponents;

public class AdminUserDropdownViewComponent : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        Claim? idClaim = UserClaimsPrincipal.Claims.Where(c => c.Type == "Id").FirstOrDefault()
            ?? throw new Exception("Current user Id claim not found");
        Claim? nameClaim = UserClaimsPrincipal.Claims.Where(c => c.Type == ClaimTypes.Name).FirstOrDefault()
            ?? throw new Exception("Current user Name claim not found");

        AdminUserDropdown model = new()
        {
            Id = int.Parse(idClaim.Value),
            Username = nameClaim.Value
        };

        return View(model);
    }
}
