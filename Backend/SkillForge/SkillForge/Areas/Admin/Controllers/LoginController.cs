using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using System.Net;
using SkillForge.Areas.Admin.Models.Components.Common;
using SkillForge.Models.Database;
using SkillForge.Areas.Admin.Models.DTOs;
using SkillForge.Areas.Admin.Services;

namespace SkillForge.Areas.Admin.Controllers;

public class LoginController : AdminController
{
    private readonly IAdminAuthService authService;

    public LoginController(IAdminAuthService authService)
    {
        this.authService = authService;
    }

    [AllowAnonymous]
    public async Task<IActionResult> Index(string? returnUrl = null)
    {
        if (User.Identity != null && User.Identity.IsAuthenticated)
        {
            return Redirect("/Admin");
        }

        int adminUserCount = await authService.GetAdminUserCount();

        if (adminUserCount == 0)
        {
            return Redirect("/Admin/Install");
        }

        ViewData["ReturnUrl"] = returnUrl;
        AdminUserLoginDTO loginModel = new();

        return View(loginModel);
    }

    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> Auth([Bind] AdminUserLoginDTO loginModel, string? returnUrl = null)
    {
        AdminUser? user = await authService.Authenticate(loginModel);

        if (user != null)
        {
            ClaimsPrincipal principal = authService.CreateClaimsPrincipal(user);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            if (returnUrl != null)
            {
                return Redirect(WebUtility.UrlDecode(returnUrl));
            }

            return Redirect("/Admin");
        }

        Alert("Invalid credentials.", ColorClass.Danger);

        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        return RedirectToAction("Index");
    }
}
