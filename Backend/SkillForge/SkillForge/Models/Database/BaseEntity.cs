using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using SkillForge.Attributes;

namespace SkillForge.Models.Database;

public abstract class BaseEntity : IBaseEntity
{
    [Column(Order = 0)]
    [TableColumn(Name = "#", SortOrder = -99)]
    public int Id { get; set; }

    [JsonIgnore]
    [Column(Order = 98)]
    [DataType(DataType.DateTime)]
    [TableColumn(SortOrder = 998)]
    [Display(Name = "Updated at")]
    public DateTime? UpdatedAt { get; set; }

    [JsonIgnore]
    [Column(Order = 99)]
    [DataType(DataType.DateTime)]
    [TableColumn(SortOrder = 999)]
    [Display(Name = "Created at")]
    public DateTime? CreatedAt { get; set; }
}
