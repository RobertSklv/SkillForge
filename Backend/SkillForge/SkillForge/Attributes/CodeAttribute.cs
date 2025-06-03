using System.ComponentModel.DataAnnotations;

namespace SkillForge.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class CodeAttribute : RegularExpressionAttribute
{
    public const string PATTERN = "^(?:[a-z][a-z0-9]*(?:_[a-z][a-z0-9]*)*)$";

    public CodeAttribute()
        : base(PATTERN)
    {
        ErrorMessage = "Invalid code.";
    }
}
