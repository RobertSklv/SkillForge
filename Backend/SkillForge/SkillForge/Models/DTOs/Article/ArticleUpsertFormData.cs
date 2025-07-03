using System.ComponentModel.DataAnnotations;

namespace SkillForge.Models.DTOs.Article;

public class ArticleUpsertFormData
{
    public int Id { get; set; }

    [StringLength(64)]
    public string? Image { get; set; }

    [StringLength(64)]
    public string Title { get; set; }

    [StringLength(8000)]
    public string Content { get; set; }

    [MaxLength(3, ErrorMessage = "Only up to 3 tags are allowed per article.")]
    public List<string> Tags { get; set; }
}
