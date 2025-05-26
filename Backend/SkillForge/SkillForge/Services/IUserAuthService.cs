using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using SkillForge.Models.Database;
using SkillForge.Models.DTOs;

namespace SkillForge.Services;

public interface IUserAuthService
{
    Task<User?> Authenticate(UserLoginCredentials creds);

    ClaimsPrincipal CreateClaimsPrincipal(User user);

    Task<bool> IsUsernameTaken(string username);

    Task<bool> IsEmailTaken(string email);

    Task<User?> Register(UserRegisterCredentials creds, ModelStateDictionary modelState);
}