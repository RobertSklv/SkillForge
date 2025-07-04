﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using SkillForge.Attributes;

namespace SkillForge.Models.Database;

[SelectOption(LabelProperty = nameof(Title))]
public class Article : BaseEntity
{
    [DeleteBehavior(DeleteBehavior.NoAction)]
    [TableColumn]
    public User? Author { get; set; }

    public int AuthorId { get; set; }

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

    public List<ArticleReport>? Reports { get; set; }

    [Column(TypeName = "tinyint")]
    [TableColumn(Name = "Delete reason")]
    public Violation? DeleteReason { get; set; }

    [TableColumn(Name = "Views")]
    public int ViewCount { get; set; }

    [TableColumn(Name = "Thumbs up")]
    public int ThumbsUp { get; set; }

    [TableColumn(Name = "Thumbs down")]
    public int ThumbsDown { get; set; }
}
