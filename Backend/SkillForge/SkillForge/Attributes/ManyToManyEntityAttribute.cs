namespace SkillForge.Attributes;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class ManyToManyEntityAttribute : Attribute
{
    public readonly string leftSide;
    public readonly string rightSide;

    public ManyToManyEntityAttribute(string leftSide, string rightSide)
    {
        this.leftSide = leftSide;
        this.rightSide = rightSide;
    }
}