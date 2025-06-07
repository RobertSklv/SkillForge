using SkillForge.Areas.Admin.Repositories;
using SkillForge.Models.Database;
using SkillForge.Models.DTOs.User;
using SkillForge.Services;

namespace SkillForge.Areas.Admin.Services;

public class UserService : CrudService<User>, IUserService
{
    private readonly IUserRepository repository;
    private readonly IFrontendService frontendService;

    public UserService(IUserRepository repository, IFrontendService frontendService)
        : base(repository)
    {
        this.repository = repository;
        this.frontendService = frontendService;
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

    public Task<List<User>> GetMostPopular()
    {
        return repository.GetMostPopular();
    }

    public async Task<List<UserLink>> GetMostPopularLinks()
    {
        List<User> users = await GetMostPopular();

        return users.ConvertAll(frontendService.CreateUserLink);
    }

    public Task<List<UserFollow>> GetFollowings(int id)
    {
        return repository.GetFollowings(id);
    }

    public Task<List<UserFollow>> GetFollowers(int id)
    {
        return repository.GetFollowers(id);
    }
}
