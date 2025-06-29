using System.ComponentModel.DataAnnotations;
using SkillForge.Attributes;

namespace SkillForge.Areas.Admin.Models.DTOs;

public class AdminUserVM : IModel
{
    public int Id { get; set; }

    [Required]
    [StringLength(16, MinimumLength = 1)]
    [Username]
    public string Username { get; set; }

    [Required]
    [Display(Name = "E-mail")]
    [EmailAddress(ErrorMessage = "Must enter a valid e-mail address.")]
    public string Email { get; set; }

    [DataType(DataType.Password)]
    public string? Password { get; set; }

    [Display(Name = "Confirm Password")]
    [DataType(DataType.Password)]
    [Compare(nameof(Password), ErrorMessage = "Passwords don't match")]
    public string? ConfirmPassword { get; set; }

    [Display(Name = "Current Password")]
    [DataType(DataType.Password)]
    public string? CurrentPassword { get; set; }

    public IFormFile? AvatarImage { get; set; }

    public string? CurrentAvatarFilename { get; set; }

    public bool RemoveAvatarImage { get; set; }

    [Required]
    [Display(Name = "Role")]
    public int RoleId { get; set; }
}
