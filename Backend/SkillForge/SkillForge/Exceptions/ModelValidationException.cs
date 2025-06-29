namespace SkillForge.Exceptions;

public class ModelValidationException : Exception
{
	public string FieldName { get; private set; }

	public ModelValidationException(string fieldName, string msg)
        : base($"{fieldName}: {msg}")
    {
        FieldName = fieldName;
    }
}
