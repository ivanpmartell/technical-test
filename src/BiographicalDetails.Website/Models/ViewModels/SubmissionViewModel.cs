using BiographicalDetails.Website.Models.Enums;

namespace BiographicalDetails.Website.Models.ViewModels;

public class SubmissionViewModel
{
	public int Id { get; set; }
	public required string FirstName { get; set; }
	public required string LastName { get; set; }
	public required string Email { get; set; }
	public LevelOfStudy LevelOfStudy { get; set; }
	public ImmigrationStatus ImmigrationStatus { get; set; }
	public string? SocialInsuranceNumber { get; set; }
	public string? UniqueClientIdentifier { get; set; }
}