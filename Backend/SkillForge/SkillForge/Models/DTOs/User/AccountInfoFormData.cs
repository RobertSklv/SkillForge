using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace SkillForge.Models.DTOs.User;

public class AccountInfoFormData
{
    [StringLength(32)]
    [EmailAddress]
    [Display(Name = "E-mail")]
    public string Email { get; set; }

    [StringLength(64)]
    [Display(Name = "Avatar image")]
    public string? AvatarImage { get; set; }

    [StringLength(256)]
    public string? Bio { get; set; }

    [JsonIgnore]
    public DateTime? _DateOfBirth { get; set; }

    [Display(Name = "Date of birth")]
    public string? DateOfBirth
    {
        get => $"{_DateOfBirth:yyyy-MM-dd}";
        set
        {
            if (!string.IsNullOrEmpty(value))
            {
                _DateOfBirth = DateTime.Parse(value);
            }
        }
    }
}
