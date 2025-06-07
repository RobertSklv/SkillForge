namespace SkillForge.Exceptions;

public class RecordNotFoundException : Exception
{
	public RecordNotFoundException(string msg) : base(msg)
	{
	}

	public RecordNotFoundException(int id) : this($"No record with ID {id} exists.")
	{
    }

    public RecordNotFoundException() : this("Record not found.")
    {
    }
}
