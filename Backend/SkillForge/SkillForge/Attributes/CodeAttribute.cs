using System.ComponentModel.DataAnnotations;

namespace SkillForge.Attributes;

public class CodeAttribute : RegularExpressionAttribute
{
    public CodeAttribute()
        : base("^(?:[a-z][a-z0-9]*(?:_[a-z][a-z0-9]*)*)$")
    {
        ErrorMessage = "Invalid code.";
    }
}
