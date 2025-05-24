using System.ComponentModel.DataAnnotations;

namespace SkillForge.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class UsernameAttribute : RegularExpressionAttribute
{
    public UsernameAttribute()
        : base(@"^([a-zA-Z0-9\-_]{1,16})$")
    {
        ErrorMessage = "Must enter a valid username. It must be 1-16 characters long and only the following characters are allowed: a-z, A-Z, 0-9, -, _.";
    }
}
