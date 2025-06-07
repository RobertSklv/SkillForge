namespace SkillForge.Exceptions;

public class AlreadyFollowingException : Exception
{
	public AlreadyFollowingException(string msg) : base(msg)
	{
    }

    public AlreadyFollowingException() : this("Already following")
    {
    }
}
