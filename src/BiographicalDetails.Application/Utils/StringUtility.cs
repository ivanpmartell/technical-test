namespace BiographicalDetails.Application.Utils;

public static class StringUtility
{
	public static string? EnsureNullIfEmpty(string? value)
	{
		return String.IsNullOrEmpty(value) ?
			null : value;
	}
}
