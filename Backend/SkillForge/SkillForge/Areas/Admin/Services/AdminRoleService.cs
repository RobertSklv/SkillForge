using SkillForge.Areas.Admin.Repositories;
using SkillForge.Models.Database;

namespace SkillForge.Areas.Admin.Services;

public class AdminRoleService : CrudService<AdminRole>, IAdminRoleService
{
    private readonly IAdminRoleRepository repository;

    public AdminRoleService(IAdminRoleRepository repository)
        : base(repository)
    {
        this.repository = repository;
    }

    public Task<AdminRole> Get(string code)
    {
        return repository.Get(code);
    }
}
