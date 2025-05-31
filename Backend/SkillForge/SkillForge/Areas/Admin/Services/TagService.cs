using SkillForge.Areas.Admin.Models.Components.Grid;
using SkillForge.Areas.Admin.Models.DTOs;
using SkillForge.Areas.Admin.Models;
using SkillForge.Areas.Admin.Repositories;
using SkillForge.Models.Database;

namespace SkillForge.Areas.Admin.Services;

public class TagService : CrudService<Tag>, ITagService
{
    private readonly ITagRepository repository;

    public TagService(ITagRepository repository)
        : base(repository)
    {
        this.repository = repository;
    }
}
