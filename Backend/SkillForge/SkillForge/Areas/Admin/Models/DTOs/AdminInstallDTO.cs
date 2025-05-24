using System.ComponentModel.DataAnnotations;

namespace SkillForge.Areas.Admin.Models.DTOs;

public class AdminInstallDTO
{
    [Required]
    [StringLength(32)]
    [DataType(DataType.Password)]
    public string Key { get; set; }
}