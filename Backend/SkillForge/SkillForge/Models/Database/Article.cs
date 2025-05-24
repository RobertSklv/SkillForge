using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SkillForge.Models.Database;

public class Article : BaseEntity
{
    [DeleteBehavior(DeleteBehavior.NoAction)]
    public User? Author { get; set; }

    public int AuthorId { get; set; }

    public Category? Category { get; set; }

    public int CategoryId { get; set; }

    [StringLength(64)]
    [Column(TypeName = "varchar")]
    public string? Image { get; set; }

    [StringLength(64)]
    public string Title { get; set; }

    [StringLength(8000)]
    public string Content { get; set; }

    public ArticleApproval? Approval { get; set; }

    public int? ApprovalId { get; set; }

    public List<ArticleView>? Views { get; set; }

    public List<ArticleRating>? Ratings { get; set; }

    public List<Comment>? Comments { get; set; }

    public List<ArticleTag>? Tags { get; set; }
}
