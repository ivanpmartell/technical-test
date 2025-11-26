

namespace BiographicalDetails.Application.Errors;

public static class BiographicalDetailsErrors
{
	public const string RequiredSIN_Missing = "RequiredSIN_Missing";
	public const string RequiredUCI_Missing = "RequiredUCI_Missing";
	public const string BiographicalDetails_NotFound = "BiographicalDetails_NotFound";
	public const string UCIFormat_Invalid = "UCIFormat_Invalid";
	public const string SINFormat_Invalid = "SINFormat_Invalid";
	public const string SIN_Invalid = "SIN_Invalid";
	public const string AlreadySet_SIN_CannotUpdate = "AlreadySet_SIN_CannotUpdate";
	public const string AlreadySet_UCI_CannotUpdate = "AlreadySet_UCI_CannotUpdate";
}
