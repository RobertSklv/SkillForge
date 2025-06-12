namespace SkillForge.Models.DTOs.Search;

public class PaginationResponse<T>
    where T : class
{
    public List<T> Items { get; set; }

    public int ItemCount { get; set; }

    public int TotalItems { get; set; }
}
