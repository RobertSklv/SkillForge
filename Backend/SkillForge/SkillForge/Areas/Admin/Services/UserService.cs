using SkillForge.Areas.Admin.Repositories;
using SkillForge.Models.Database;
using SkillForge.Models.DTOs;

namespace SkillForge.Areas.Admin.Services;

public class UserService : CrudService<User>, IUserService
{
    private readonly IUserRepository repository;

    public UserService(IUserRepository repository)
        : base(repository)
    {
        this.repository = repository;
    }

    public Task<User?> FindUser(string usernameOrEmail)
    {
        return repository.FindUser(usernameOrEmail);
    }

    public Task<bool> IsUsernameTaken(string username)
    {
        return repository.IsUsernameTaken(username);
    }

    public Task<bool> IsEmailTaken(string email)
    {
        return repository.IsEmailTaken(email);
    }

    public UserInfo GetUserInfo(User user)
    {
        return new UserInfo
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            AvatarPath = user.AvatarPath
        };
    }
}
