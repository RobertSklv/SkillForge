using System.ComponentModel.DataAnnotations;
using SkillForge.Attributes;

namespace SkillForge.Areas.Admin.Models.DTOs;

public class AdminUserRegisterDTO
{
    [Required]
    [StringLength(16, MinimumLength = 1)]
    [Username]
    public string Username { get; set; }

    [Required]
    [Display(Name = "E-mail")]
    [EmailAddress(ErrorMessage = "Must enter a valid e-mail address.")]
    public string Email { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [Required]
    [Display(Name = "Confirm Password")]
    [DataType(DataType.Password)]
    [Compare(nameof(Password), ErrorMessage = "Passwords don't match")]
    public string ConfirmPassword { get; set; }
}
