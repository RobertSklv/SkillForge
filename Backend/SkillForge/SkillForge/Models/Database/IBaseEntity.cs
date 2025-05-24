using SkillForge.Areas.Admin.Models;

namespace SkillForge.Models.Database;

public interface IBaseEntity : IModel
{
    DateTime? UpdatedAt { get; set; }

    DateTime? CreatedAt { get; set; }
}