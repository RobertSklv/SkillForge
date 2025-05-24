using SkillForge.Areas.Admin.Models.DTOs;
using SkillForge.Models.Database;
using System.Security.Claims;

namespace SkillForge.Areas.Admin.Services;

public interface IAdminAuthService
{
    Task<AdminUser?> Authenticate(AdminUserLoginDTO loginModel);

    ClaimsPrincipal CreateClaimsPrincipal(AdminUser user);

    Task<bool> IsUsernameTaken(string username);

    Task<bool> IsEmailTaken(string email);

    Task<int> GetAdminUserCount();

    Task<bool> CreateAdminUser(AdminUserRegisterDTO registerModel);
}