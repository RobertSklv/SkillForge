namespace SkillForge.Models.DTOs.User;

public class LoginResponse
{
    public UserInfo CurrentUserInfo { get; set; }

    public string AuthToken { get; set; }
}
