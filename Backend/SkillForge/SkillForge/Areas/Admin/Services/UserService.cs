using SkillForge.Areas.Admin.Repositories;
using SkillForge.Models.Database;

namespace SkillForge.Areas.Admin.Services;

public class UserService : CrudService<User>, IUserService
{
    private readonly IUserRepository repository;

    public UserService(IUserRepository repository)
        : base(repository)
    {
        this.repository = repository;
    }
}
