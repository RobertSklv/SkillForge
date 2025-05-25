using System.ComponentModel.DataAnnotations;

namespace SkillForge.Models.DTOs;

public class UserLoginCredentials
{
    [Required]
    [Display(Name = "Username or e-mail")]
    [DataType(DataType.Text)]
    public string UsernameOrEmail { get; set; }

    [Required]
    [StringLength(32)]
    [DataType(DataType.Password)]
    public string Password { get; set; }
}
