namespace SkillForge.Exceptions;

public class AlreadyNotFollowingException : Exception
{
	public AlreadyNotFollowingException(string msg) : base(msg)
	{
	}

	public AlreadyNotFollowingException() : this("Already not following")
	{
	}
}
