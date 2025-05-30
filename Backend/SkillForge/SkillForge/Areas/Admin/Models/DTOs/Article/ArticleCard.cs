namespace SkillForge.Areas.Admin.Models.DTOs.Article;

public class ArticleCard
{
    public int ArticleId { get; set; }

    public UserLink Author { get; set; }

    public string Title { get; set; }

    public string? CoverImage { get; set; }

    public string CategoryName { get; set; }

    public string CategoryCode { get; set; }

    public DateTime DatePublished { get; set; }

    public byte Rating { get; set; }
}
