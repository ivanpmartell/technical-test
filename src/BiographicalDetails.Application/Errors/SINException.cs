namespace BiographicalDetails.Application.Errors;

public class SINException : Exception
{
	public SINException(string message) : base(message) { }

	public SINException(string message, Exception inner) : base(message, inner) { }
}
