using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using SkillForge.Attributes;

namespace SkillForge.Models.Database;

public class Article : BaseEntity
{
    [DeleteBehavior(DeleteBehavior.NoAction)]
    [TableColumn]
    public User? Author { get; set; }

    public int AuthorId { get; set; }

    [TableColumn]
    public Category? Category { get; set; }

    public int CategoryId { get; set; }

    [StringLength(64)]
    [Column(TypeName = "varchar")]
    public string? Image { get; set; }

    [TableColumn]
    [StringLength(64)]
    public string Title { get; set; }

    [TableColumn]
    [StringLength(8000)]
    public string Content { get; set; }

    public ArticleApproval? Approval { get; set; }

    public int? ApprovalId { get; set; }

    public List<ArticleView>? Views { get; set; }

    public List<ArticleRating>? Ratings { get; set; }

    public List<Comment>? Comments { get; set; }

    public List<ArticleTag>? Tags { get; set; }

    [TableColumn]
    public int ViewCount { get; set; }

    [TableColumn]
    public int ThumbsUp { get; set; }

    [TableColumn]
    public int ThumbsDown { get; set; }
}
