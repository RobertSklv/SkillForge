using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkillForge.Areas.Admin.Models.Components.Common;
using SkillForge.Areas.Admin.Models.DTOs;
using SkillForge.Areas.Admin.Services;

namespace SkillForge.Areas.Admin.Controllers;

[Authorize(AuthenticationSchemes = "AdminInstallCookie")]
public class InstallController : AdminController
{
    private readonly IInstallService service;
    private readonly IAdminAuthService authService;

    public InstallController(
        IInstallService service,
        IAdminAuthService authService)
    {
        this.service = service;
        this.authService = authService;
    }

    [AllowAnonymous]
    public async Task<IActionResult> Index()
    {
        if (User.Identity != null && User.Identity.IsAuthenticated)
        {
            return RedirectToAction(nameof(CreateAdminAccount));
        }

        int adminUserCount = await authService.GetAdminUserCount();

        if (adminUserCount > 0)
        {
            return Redirect("/Admin/Login");
        }

        return View();
    }

    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> Auth(string key)
    {
        bool success = service.Authenticate(key);

        if (success)
        {
            List<Claim> claims = new()
            {
                new Claim(ClaimTypes.Name, "installer"),
            };
            ClaimsIdentity identity = new(claims, "AdminInstallCookie");
            ClaimsPrincipal principal = new(identity);
            await HttpContext.SignInAsync("AdminInstallCookie", principal);

            return RedirectToAction(nameof(CreateAdminAccount));
        }

        Alert("Invalid key.", ColorClass.Danger);

        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult CreateAdminAccount()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> CreateAdminAccount(AdminUserRegisterDTO model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        bool success = await authService.CreateAdminUser(model);

        if (!success)
        {
            Alert("Something went wrong while creating the admin user.", ColorClass.Danger);

            return View(model);
        }

        Alert("Admin user created successfully", ColorClass.Success);

        return Redirect("/Admin/Login");
    }
}