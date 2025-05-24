namespace SkillForge.Models.Database;

public interface IBaseEntity
{
    int Id { get; set; }

    DateTime? UpdatedAt { get; set; }

    DateTime? CreatedAt { get; set; }
}