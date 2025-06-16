namespace SkillForge.Exceptions;

public class NotOwnedByUserException : Exception
{
	public NotOwnedByUserException(string msg) : base(msg)
	{
	}
}
