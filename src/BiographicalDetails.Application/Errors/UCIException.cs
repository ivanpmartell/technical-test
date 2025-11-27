namespace BiographicalDetails.Application.Errors;

public class UCIException : Exception
{
	public UCIException(string message) : base(message) { }

	public UCIException(string message, Exception inner) : base(message, inner) { }
}
