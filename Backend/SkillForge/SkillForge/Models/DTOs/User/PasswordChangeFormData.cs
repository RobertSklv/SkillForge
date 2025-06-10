using SkillForge.Attributes;
using System.ComponentModel.DataAnnotations;

namespace SkillForge.Models.DTOs.User;

public class PasswordChangeFormData
{
    [Required]
    [DataType(DataType.Password)]
    [Password]
    public string Password { get; set; }

    [Required]
    [Display(Name = "Confirm Password")]
    [DataType(DataType.Password)]
    [Compare(nameof(Password), ErrorMessage = "Passwords don't match")]
    public string ConfirmPassword { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string CurrentPassword { get; set; }
}
