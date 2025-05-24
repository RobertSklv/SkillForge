using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkillForge.Areas.Admin.Models.Components.Common;
using SkillForge.Extensions;

namespace SkillForge.Areas.Admin.Controllers;

[Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
[Area("Admin")]
public abstract class AdminController : Controller
{
    protected void Alert(string message, ColorClass color)
    {
        Alert alert = new()
        {
            Content = message,
            Color = color
        };

        List<Alert> alerts = TempData.Get<List<Alert>>("Alerts") ?? new();
        alerts.Add(alert);
        TempData.Set("Alerts", alerts);
    }
}
