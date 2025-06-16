namespace SkillForge.Models.DTOs.User;

public class Author
{
    public UserLink Link { get; set; }

    public string? Bio { get; set; }

    public DateTime DateJoined { get; set; }

    public bool IsFollowedByCurrentUser { get; set; }
}
