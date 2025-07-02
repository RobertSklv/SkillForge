using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkillForge.Models.Database;
using SkillForge.Models.DTOs.User;
using SkillForge.Services;
using SkillForge.Areas.Admin.Services;
using SkillForge.Exceptions;
using SkillForge.Models.DTOs.Tag;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace SkillForge.Controllers;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class UserController : ApiController
{
    private readonly IUserService service;
    private readonly IUserAuthService userAuthService;
    private readonly IFrontendService frontendService;
    private readonly IUserFeedService userFeedService;

    public UserController(
        IUserService service,
        IUserAuthService userAuthService,
        IFrontendService frontendService,
        IUserFeedService userFeedService)
    {
        this.service = service;
        this.userAuthService = userAuthService;
        this.frontendService = frontendService;
        this.userFeedService = userFeedService;
    }

    [AllowAnonymous]
    [HttpGet]
    [Route("/Api/User/Load/{name}")]
    public async Task<IActionResult> Load([FromRoute] string name)
    {
        TryGetUserId(out int? userId);

        try
        {
            UserPageData pageData = await service.LoadPage(name, userId);

            return Ok(pageData);
        }
        catch (RecordNotFoundException e)
        {
            return NotFound(e.Message);
        }
    }

    [AllowAnonymous]
    [HttpGet]
    [Route("/Api/User/Followers/{name}")]
    public async Task<IActionResult> Followers([FromRoute] string name, [FromQuery] int batchIndex, [FromQuery] int batchSize)
    {
        User? u = await service.GetByName(name);

        if (u == null) return NotFound("User not found");

        TryGetUserId(out int? userId);

        List<UserListItem> followers = await userFeedService.GetUserFollowers(u.Id, userId, batchIndex, batchSize);

        return Ok(followers);
    }

    [AllowAnonymous]
    [HttpGet]
    [Route("/Api/User/Followings/{name}")]
    public async Task<IActionResult> Followings([FromRoute] string name, [FromQuery] int batchIndex, [FromQuery] int batchSize)
    {
        User? u = await service.GetByName(name);

        if (u == null) return NotFound("User not found");

        TryGetUserId(out int? userId);

        List<UserListItem> followings = await userFeedService.GetUserFollowings(u.Id, userId, batchIndex, batchSize);

        return Ok(followings);
    }

    [AllowAnonymous]
    [HttpGet]
    [Route("/Api/User/TagFollowings/{name}")]
    public async Task<IActionResult> TagFollowings([FromRoute] string name, [FromQuery] int batchIndex, [FromQuery] int batchSize)
    {
        User? u = await service.GetByName(name);

        if (u == null) return NotFound("User not found");

        TryGetUserId(out int? userId);

        List<TagListItem> tagfollowings = await userFeedService.GetUserTagFollowings(u.Id, userId, batchIndex, batchSize);

        return Ok(tagfollowings);
    }

    [HttpPost]
    public async Task<IActionResult> Follow(UserRequest userRequest)
    {
        if (!TryGetUserId(out int? userId))
        {
            return Unauthorized();
        }

        try
        {
            await service.Follow((int)userId, userRequest.User);
        }
        catch (RecordNotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (AlreadyFollowingException e)
        {
            return BadRequest(e.Message);
        }

        return Ok();
    }

    [HttpDelete]
    public async Task<IActionResult> Unfollow(UserRequest userRequest)
    {
        if (!TryGetUserId(out int? userId))
        {
            return Unauthorized();
        }

        try
        {
            await service.Unfollow((int)userId, userRequest.User);
        }
        catch (RecordNotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (AlreadyNotFollowingException e)
        {
            return BadRequest(e.Message);
        }

        return Ok();
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
                SetExpiredAuthTokenCookie();
            }
            else
            {
                if (await service.IsSuspended(user.Id))
                {
                    SetExpiredAuthTokenCookie();

                    return BadRequest("Your account is temporarily suspended");
                }

                return Ok(frontendService.GetUserInfo(user));
            }
        }

        return Unauthorized();
    }

    [HttpGet]
    public async Task<IActionResult> AccountInfoForm()
    {
        if (TryGetUserId(out int? userId))
        {
            User user = await service.GetStrict((int)userId);

            return Ok(new AccountInfoFormData
            {
                Email = user.Email,
                AvatarImage = user.AvatarPath,
                Bio = user.Bio,
                _DateOfBirth = user.DateOfBirth,
            });
        }

        return Unauthorized();
    }

    [HttpPost]
    public async Task<IActionResult> UpdateInfo(AccountInfoFormData formData)
    {
        if (TryGetUserId(out int? userId))
        {
            if (await service.UpdateInfo((int)userId, formData))
            {
                return Ok("Account information successfully updated!");
            }

            return BadRequest("Something went wrong!");
        }

        return Unauthorized();
    }

    [HttpPost]
    public async Task<IActionResult> UpdatePassword(PasswordChangeFormData formData)
    {
        if (TryGetUserId(out int? userId))
        {
            if (await service.UpdatePassword((int)userId, formData, ModelState))
            {
                return Ok("Account information successfully updated!");
            }

            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            return BadRequest("Something went wrong!");
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
            return ValidationProblem("Invalid credentials.");
        }

        if (await service.IsSuspended(user.Id))
        {
            return ValidationProblem("Your account is temporarily suspended");
        }

        string token = userAuthService.GenerateToken(user);

        SetAuthTokenCookie(token);

        return Ok(frontendService.GetUserInfo(user));
    }

    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> Register([FromBody] UserRegisterCredentials creds)
    {
        User? user = await userAuthService.Register(creds, ModelState);

        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        if (user == null)
        {
            return BadRequest("Registration unsuccessful.");
        }

        string token = userAuthService.GenerateToken(user);

        SetAuthTokenCookie(token);

        return Ok("Registration successful.");
    }

    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        SetExpiredAuthTokenCookie();

        return Ok("Logged out successfully.");
    }

    [AllowAnonymous]
    public async Task<IActionResult> VerifyUniqueUsername(string name)
    {
        return Json(!await service.IsUsernameTaken(name));
    }

    [AllowAnonymous]
    public async Task<IActionResult> VerifyUniqueEmail(string email)
    {
        return Json(!await service.IsEmailTaken(email));
    }

    public void SetAuthTokenCookie(string token)
    {
        Response.Cookies.Append("auth_jwt", token, new CookieOptions
        {
            SameSite = SameSiteMode.None,
            Secure = true,
            HttpOnly = false
        });
    }

    public void SetExpiredAuthTokenCookie()
    {
        Response.Cookies.Append("auth_jwt", "", new CookieOptions
        {
            Expires = DateTimeOffset.UtcNow.AddDays(-1),
            SameSite = SameSiteMode.None,
            Secure = true,
            HttpOnly = false
        });
    }
}
