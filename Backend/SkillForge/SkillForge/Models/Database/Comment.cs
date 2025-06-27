using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using SkillForge.Attributes;

namespace SkillForge.Models.Database;

public class Comment : BaseEntity
{
    [DeleteBehavior(DeleteBehavior.NoAction)]
    [TableColumn(Name = "Author")]
    public User? User { get; set; }

    public int UserId { get; set; }

    [TableColumn]
    public Article? Article { get; set; }

    public int ArticleId { get; set; }

    [StringLength(8000)]
    [TableColumn]
    public string Content { get; set; }

    public List<CommentRating>? Ratings { get; set; }

    [Column(TypeName = "tinyint")]
    [TableColumn(Name = "Delete reason")]
    public Violation? DeleteReason { get; set; }

    [TableColumn(Name = "Thumbs up")]
    public int ThumbsUp { get; set; }

    [TableColumn(Name = "Thumbs down")]
    public int ThumbsDown { get; set; }

    public DateTime? ContentEditedAt { get; set; }
}
