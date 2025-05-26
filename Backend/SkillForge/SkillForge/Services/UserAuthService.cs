using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using SkillForge.Areas.Admin.Models.DTOs;
using SkillForge.Areas.Admin.Repositories;
using SkillForge.Models.Database;
using SkillForge.Models.DTOs;
using System.Security.Claims;

namespace SkillForge.Services;

public class UserAuthService : IUserAuthService
{
    private readonly IUserRepository userRepository;
    private readonly IAuthService authService;

    public UserAuthService(
        IUserRepository userRepository,
        IAuthService authService)
    {
        this.userRepository = userRepository;
        this.authService = authService;
    }

    public async Task<User?> Authenticate(UserLoginCredentials creds)
    {
        User? user = await userRepository.FindUser(creds.UsernameOrEmail);

        if (user != null)
        {
            if (authService.CompareHashes(creds.Password, user.PasswordHash, user.PasswordHashSalt))
            {
                return user;
            }
        }

        return null;
    }

    public ClaimsPrincipal CreateClaimsPrincipal(User user)
    {
        List<Claim> claims = new()
        {
            new Claim(ClaimTypes.NameIdentifier, user.Name),
            new Claim(ClaimTypes.Name, user.Name),
            new Claim(ClaimTypes.Email, user.Email),
        };
        ClaimsIdentity identity = new(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        ClaimsPrincipal principal = new(identity);

        return principal;
    }

    public async Task<bool> IsUsernameTaken(string username)
    {
        return await userRepository.IsUsernameTaken(username);
    }

    public async Task<bool> IsEmailTaken(string email)
    {
        return await userRepository.IsEmailTaken(email);
    }

    public async Task<User?> Register(UserRegisterCredentials creds, ModelStateDictionary modelState)
    {
        bool isValid = true;

        if (await userRepository.IsUsernameTaken(creds.Username))
        {
            modelState.AddModelError(nameof(UserRegisterCredentials.Username), $"{nameof(UserRegisterCredentials.Username)} is already taken.");
            isValid = false;
        }

        if (await userRepository.IsEmailTaken(creds.Email))
        {
            modelState.AddModelError(nameof(UserRegisterCredentials.Email), "An account with this e-mail already exists.");
            isValid = false;
        }

        if (isValid)
        {
            byte[] passwordHashSalt = authService.GenerateSalt();
            string passwordHash = authService.Hash(creds.Password, passwordHashSalt);

            User user = new()
            {
                Name = creds.Username,
                Email = creds.Email,
                PasswordHash = passwordHash,
                PasswordHashSalt = passwordHashSalt
            };

            if (await userRepository.Upsert(user) > 0)
            {
                return user;
            }
        }

        return null;
    }
}
