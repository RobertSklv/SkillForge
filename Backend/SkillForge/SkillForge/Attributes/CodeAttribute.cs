using System.ComponentModel.DataAnnotations;

namespace SkillForge.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class CodeAttribute : RegularExpressionAttribute
{
    public CodeAttribute()
        : base("^(?:[a-z][a-z0-9]*(?:_[a-z][a-z0-9]*)*)$")
    {
        ErrorMessage = "Invalid code.";
    }
}
