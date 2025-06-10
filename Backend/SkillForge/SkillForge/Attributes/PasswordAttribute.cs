using System.ComponentModel.DataAnnotations;

namespace SkillForge.Attributes;

public class PasswordAttribute : RegularExpressionAttribute
{
    public PasswordAttribute()
        : base("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\W).{8,}$")
    {
        ErrorMessage = "The password must be at least 8 characters long, contain at least one lowercase and uppercase letter, and at least one symbol.";
    }
}
