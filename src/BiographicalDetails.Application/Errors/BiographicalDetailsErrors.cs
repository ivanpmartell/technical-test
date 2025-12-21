namespace BiographicalDetails.Application.Errors;

public static class BiographicalDetailsErrors
{
	public const string RequiredSIN_Missing = "Social Insurance Number is required";
	public const string RequiredUCI_Missing = "Unique Client Identifier is required";
	public const string UCIFormat_Invalid = "Unique client identifier must have the following formats: 0000-0000 or 00-0000-0000";
	public const string SINFormat_Invalid = "Social insurance number must have the following format: 000-000-000";
	public const string SIN_Invalid = "Social insurance number is invalid";
	public const string AlreadySet_SIN_CannotUpdate = "Cannot update the Social Insurance Number";
	public const string AlreadySet_UCI_CannotUpdate = "Cannot update the Unique Client Identifier";

	public const string BiographicalDetails_NotFound = "BiographicalDetails_NotFound";
	public const string BiographicalDetails_NotAdded = "BiographicalDetails_NotAdded";
}
