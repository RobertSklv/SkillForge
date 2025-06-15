using Microsoft.AspNetCore.Mvc.ModelBinding;
using SkillForge.Areas.Admin.Repositories;
using SkillForge.Exceptions;
using SkillForge.Models.Database;
using SkillForge.Models.DTOs.Article;
using SkillForge.Models.DTOs.Tag;
using SkillForge.Models.DTOs.User;
using SkillForge.Services;

namespace SkillForge.Areas.Admin.Services;

public class UserService : CrudService<User>, IUserService
{
    private readonly IUserRepository repository;
    private readonly IFrontendService frontendService;
    private readonly IUserFeedService userFeedService;
    private readonly IUserAuthService userAuthService;
    private readonly IAuthService authService;

    public UserService(
        IUserRepository repository,
        IFrontendService frontendService,
        IUserFeedService userFeedService,
        IUserAuthService userAuthService,
        IAuthService authService)
        : base(repository)
    {
        this.repository = repository;
        this.frontendService = frontendService;
        this.userFeedService = userFeedService;
        this.userAuthService = userAuthService;
        this.authService = authService;
    }

    public Task<User?> GetByName(string name)
    {
        return repository.GetByName(name);
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

    public Task<bool> IsFollowedBy(int id, int followerId)
    {
        return repository.IsFollowedBy(id, followerId);
    }

    public Task<bool> IsFollowing(int id, int followedUserId)
    {
        return repository.IsFollowing(id, followedUserId);
    }

    public Task<List<UserFollow>> GetFollowings(int id, List<int> followedUserIds)
    {
        return repository.GetFollowings(id, followedUserIds);
    }

    public Task<List<TagFollow>> GetTagFollowings(int id, List<int> followedTagIds)
    {
        return repository.GetTagFollowings(id, followedTagIds);
    }

    public Task<List<UserFollow>> GetLatestFollowers(int userId, int batchIndex, int batchSize)
    {
        return repository.GetLatestFollowers(userId, batchIndex, batchSize);
    }

    public Task<List<UserFollow>> GetLatestFollowings(int userId, int batchIndex, int batchSize)
    {
        return repository.GetLatestFollowings(userId, batchIndex, batchSize);
    }

    public Task<List<TagFollow>> GetLatestTagFollowings(int userId, int batchIndex, int batchSize)
    {
        return repository.GetLatestTagFollowings(userId, batchIndex, batchSize);
    }

    public async Task Follow(int currentUserId, string username)
    {
        User? user = await GetByName(username) ?? throw new RecordNotFoundException($"User '{username}' does not exist.");
        User? currentUser = await GetStrict(currentUserId);

        UserFollow? followRecord = await repository.GetFollowRecord(currentUserId, user.Id);

        if (followRecord != null)
        {
            throw new AlreadyFollowingException();
        }

        UserFollow follow = new()
        {
            FollowerId = currentUserId,
            FollowedUserId = user.Id
        };

        currentUser.FollowingsCount++;
        user.FollowersCount++;

        await repository.SaveFollowRecord(follow);
    }

    public async Task Unfollow(int currentUserId, string username)
    {
        User? user = await GetByName(username) ?? throw new RecordNotFoundException($"User '{username}' does not exist.");
        User? currentUser = await GetStrict(currentUserId);

        UserFollow? followRecord = await repository.GetFollowRecord(currentUserId, user.Id) ?? throw new AlreadyNotFollowingException();

        currentUser.FollowingsCount--;
        user.FollowersCount--;

        await repository.DeleteFollowRecord(followRecord);
    }

    public async Task<bool> UpdateInfo(int userId, AccountInfoFormData formData)
    {
        User user = await GetStrict(userId);

        user.Email = formData.Email;
        user.AvatarPath = formData.AvatarImage;
        user.Bio = formData.Bio;
        user.DateOfBirth = formData._DateOfBirth;

        return await Update(user);
    }

    public async Task<bool> UpdatePassword(int userId, PasswordChangeFormData formData, ModelStateDictionary modelState)
    {
        User user = await GetStrict(userId);

        if (formData.Password == formData.CurrentPassword)
        {
            modelState.AddModelError(nameof(PasswordChangeFormData.Password), "New password cannot be the same as the old password");

            return false;
        }

        if (!authService.CompareHashes(formData.CurrentPassword, user.PasswordHash, user.PasswordHashSalt))
        {
            modelState.AddModelError(nameof(PasswordChangeFormData.CurrentPassword), "Incorrect password");

            return false;
        }

        byte[] passwordHashSalt = authService.GenerateSalt();
        string passwordHash = authService.Hash(formData.Password, passwordHashSalt);

        user.PasswordHashSalt = passwordHashSalt;
        user.PasswordHash = passwordHash;

        return await Update(user);
    }

    public async Task<UserPageData> LoadPage(string name, int? currentUserId = null)
    {
        User? user = await GetByName(name) ?? throw new RecordNotFoundException($"User '{name}' does not exist.");

        List<TopArticleItem> topArticles = await userFeedService.GetTopArticlesByAuthor(user.Id, 5);
        List<ArticleCard> latestArticles = await userFeedService.GetLatestArticlesByAuthor(name, currentUserId, 0, 4);
        List<UserListItem> latestFollowers = await userFeedService.GetLatestUserFollowers(user.Id, currentUserId, 6);
        List<UserListItem> latestFollowings = await userFeedService.GetLatestUserFollowings(user.Id, currentUserId, 6);
        List<TagListItem> latestTagFollowings = await userFeedService.GetLatestUserTagFollowings(user.Id, currentUserId, 6);

        return new UserPageData
        {
            Name = user.Name,
            Bio = user.Bio,
            AvatarImage = user.AvatarPath,
            ArticlesCount = user.ArticlesCount,
            FollowersCount = user.FollowersCount,
            FollowingsCount = user.FollowingsCount,
            TagFollowingsCount = user.TagFollowingsCount,
            IsFollowedByCurrentUser = currentUserId != null && await IsFollowedBy(user.Id, (int)currentUserId),
            LatestArticles = latestArticles,
            LatestFollowers = latestFollowers,
            LatestFollowings = latestFollowings,
            LatestTagFollowings = latestTagFollowings,
            TopArticles = topArticles,
        };
    }
}
