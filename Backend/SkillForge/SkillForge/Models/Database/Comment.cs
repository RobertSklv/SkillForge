using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace SkillForge.Models.Database;

public class Comment : BaseEntity
{
    [DeleteBehavior(DeleteBehavior.NoAction)]
    public User? User { get; set; }

    public int UserId { get; set; }

    public Article? Article { get; set; }

    public int ArticleId { get; set; }

    [StringLength(8000)]
    public string Content { get; set; }

    public List<CommentRating>? Ratings { get; set; }

    public int ThumbsUp { get; set; }

    public int ThumbsDown { get; set; }
}
