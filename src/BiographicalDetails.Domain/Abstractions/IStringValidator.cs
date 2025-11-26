namespace BiographicalDetails.Domain.Abstractions;

public interface IStringValidator
{
	public bool IsValid(string value, out string errorMessage);
}
