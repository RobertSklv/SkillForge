using SkillForge.Attributes;
using System.ComponentModel.DataAnnotations;

namespace SkillForge.Models.DTOs;

public class UserRegisterCredentials
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
    [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\W).{8,}$", ErrorMessage = "The password must be at least 8 characters long, contain at least one lowercase and letter, and at least one symbol.")]
    public string ConfirmPassword { get; set; }
}
