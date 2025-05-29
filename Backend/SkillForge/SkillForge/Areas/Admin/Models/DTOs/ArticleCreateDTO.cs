using System.ComponentModel.DataAnnotations;

namespace SkillForge.Areas.Admin.Models.DTOs;

public class ArticleCreateDTO
{
    [Range(1, int.MaxValue, ErrorMessage = "The Category field is required.")]
    public int CategoryId { get; set; }

    [StringLength(64)]
    public string? Image { get; set; }

    [StringLength(64)]
    public string Title { get; set; }

    [StringLength(8000)]
    public string Content { get; set; }
}
