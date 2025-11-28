using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using BiographicalDetails.Website.Models.Enums;
using BiographicalDetails.Website.Models.Validation;

namespace BiographicalDetails.Website.Models;

public class BiographicalDetailsModel
{
	public int Id { get; set; } = 0;

	[Required]
	[MaxLength(100)]
	[DisplayName("First name")]
	public required string FirstName { get; set; }

	[Required]
	[MaxLength(100)]
	[DisplayName("Last name")]
	public required string LastName { get; set; }

	[Required]
	[EmailAddress]
	[MaxLength(100)]
	[DisplayName("Email")]
	public required string Email { get; set; }

	[MaxLength(50)]
	[DisplayName("Preferred pronouns")]
	public string? PreferredPronouns { get; set; }

	[Required]
	[DisplayName("Level of study")]
	public LevelOfStudy LevelOfStudy { get; set; }

	[Required]
	[DisplayName("Immigration status")]
	public ImmigrationStatus ImmigrationStatus { get; set; }

	[Sin]
	[DisplayName("Social insurance number")]
	public string? SocialInsuranceNumber { get; set; }

	[Uci]
	[DisplayName("Unique client identifier")]
	public string? UniqueClientIdentifier { get; set; }
}
