using System.ComponentModel;
using BiographicalDetails.Website.Models.Enums;

namespace BiographicalDetails.Website.Models.ViewModels;

public class SubmissionViewModel
{
	public int Id { get; set; }

	[DisplayName("First Name")]
	public required string FirstName { get; set; }

	[DisplayName("Last Name")]
	public required string LastName { get; set; }

	[DisplayName("Email")]
	public required string Email { get; set; }

	[DisplayName("Level of study")]
	public LevelOfStudy LevelOfStudy { get; set; }

	[DisplayName("Immigration status")]
	public ImmigrationStatus ImmigrationStatus { get; set; }

	[DisplayName("Social insurance number")]
	public string? SocialInsuranceNumber { get; set; }

	[DisplayName("Unique client identifier")]
	public string? UniqueClientIdentifier { get; set; }
}