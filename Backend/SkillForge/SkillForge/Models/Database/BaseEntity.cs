using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SkillForge.Models.Database;

public abstract class BaseEntity : IBaseEntity
{
    [Column(Order = 0)]
    public int Id { get; set; }

    [JsonIgnore]
    [Column(Order = 98)]
    [DataType(DataType.DateTime)]
    [Display(Name = "Updated at")]
    public DateTime? UpdatedAt { get; set; }

    [JsonIgnore]
    [Column(Order = 99)]
    [DataType(DataType.DateTime)]
    [Display(Name = "Created at")]
    public DateTime? CreatedAt { get; set; }
}
