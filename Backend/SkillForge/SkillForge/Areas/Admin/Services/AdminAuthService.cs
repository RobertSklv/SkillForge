using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using SkillForge.Areas.Admin.Models.DTOs;
using SkillForge.Areas.Admin.Repositories;
using SkillForge.Models.Database;
using SkillForge.Services;

namespace SkillForge.Areas.Admin.Services;

public class AdminAuthService : IAdminAuthService
{
    private readonly IAuthService authService;
    private readonly IAdminUserRepository adminUserRepository;
    private readonly IAdminRoleService adminRoleService;

    public AdminAuthService(
        IAuthService authService,
        IAdminUserRepository adminUserRepository,
        IAdminRoleService adminRoleService)
    {
        this.authService = authService;
        this.adminUserRepository = adminUserRepository;
        this.adminRoleService = adminRoleService;
    }

    public async Task<AdminUser?> Authenticate(AdminUserLoginDTO loginModel)
    {
        AdminUser? user = await adminUserRepository.FindUser(loginModel.UsernameOrEmail);

        if (user != null)
        {
            if (authService.CompareHashes(loginModel.Password, user.PasswordHash, user.PasswordHashSalt))
            {
                return user;
            }
        }

        return null;
    }

    public ClaimsPrincipal CreateClaimsPrincipal(AdminUser user)
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
        return await adminUserRepository.IsUsernameTaken(username);
    }

    public async Task<bool> IsEmailTaken(string email)
    {
        return await adminUserRepository.IsEmailTaken(email);
    }

    public Task<int> GetAdminUserCount()
    {
        return adminUserRepository.GetAdminUserCount();
    }

    public async Task<bool> CreateAdminUser(AdminUserRegisterDTO registerModel)
    {
        bool isValid = true;

        if (!registerModel.Password.Equals(registerModel.ConfirmPassword))
        {
            isValid = false;
        }

        if (isValid)
        {
            AdminRole role = await adminRoleService.Get("admin");

            byte[] passwordHashSalt = authService.GenerateSalt();
            string passwordHash = authService.Hash(registerModel.Password, passwordHashSalt);

            AdminUser user = new()
            {
                Name = registerModel.Username,
                Email = registerModel.Email,
                RoleId = role.Id,
                PasswordHash = passwordHash,
                PasswordHashSalt = passwordHashSalt
            };

            return await adminUserRepository.SaveUser(user) > 0;
        }

        return false;
    }
}
