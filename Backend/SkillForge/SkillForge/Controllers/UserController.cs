using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkillForge.Models.Database;
using SkillForge.Models.DTOs;
using SkillForge.Services;
using SkillForge.Areas.Admin.Services;

namespace SkillForge.Controllers;

[Authorize(AuthenticationSchemes = "FrontendCookie")]
public class UserController : ApiController
{
    private readonly IUserAuthService userAuthService;
    private readonly IUserService service;

    public UserController(IUserAuthService userAuthService, IUserService userService)
    {
        this.userAuthService = userAuthService;
        this.service = userService;
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> Me()
    {
        if (User.Identity != null && User.Identity.IsAuthenticated)
        {
            string? email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

            if (email == null)
            {
                throw new Exception("E-mail claim not found");
            }

            User? user = await service.FindUser(email);

            if (user == null)
            {
                string? username = User.Claims
                    .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier || c.Type == ClaimTypes.Name)?.Value;

                if (username == null)
                {
                    throw new Exception("Username claim not found");
                }

                user = await service.FindUser(username);
            }

            if (user == null)
            {
                //If user is not found, something is wrong with the cookie, so sign the user out.
                await HttpContext.SignOutAsync("FrontendCookie");
            }
            else
            {
                return Ok(service.GetUserInfo(user));
            }
        }

        return Unauthorized();
    }

    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> Auth(UserLoginCredentials creds)
    {
        User? user = await userAuthService.Authenticate(creds);

        if (user == null)
        {
            return Unauthorized("Invalid credentials.");
        }

        ClaimsPrincipal principal = userAuthService.CreateClaimsPrincipal(user);
        await HttpContext.SignInAsync("FrontendCookie", principal);

        return Ok(service.GetUserInfo(user));
    }

    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> Register([FromBody] UserRegisterCredentials creds)
    {
        bool success = await userAuthService.Register(creds, ModelState);

        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        if (!success)
        {
            return BadRequest("Registration unsuccessful.");
        }

        return Ok("Registration successful.");
    }

    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync("FrontendCookie");

        return Ok("Logged out successfully.");
    }
}
