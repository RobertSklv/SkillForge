using SkillForge.Attributes;
using System.ComponentModel.DataAnnotations;

namespace SkillForge.Areas.Admin.Models.DTOs;

public class CategoryVM : IModel
{
    public int Id { get; set; }

    [StringLength(32, MinimumLength = 1)]
    [Code]
    public string Code { get; set; }

    [StringLength(32, MinimumLength = 1)]
    [Display(Name = "Displayed name")]
    public string DisplayedName { get; set; }

    public IFormFile? Image { get; set; }

    public string? Description { get; set; }

    public string? CurrentImageFilename { get; set; }

    public bool RemoveImage { get; set; }
}
