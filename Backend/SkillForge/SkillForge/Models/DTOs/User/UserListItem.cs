namespace SkillForge.Models.DTOs.User;

public class UserListItem
{
    public UserLink Link { get; set; }

    public bool IsFollowedByCurrentUser { get; set; }
}
