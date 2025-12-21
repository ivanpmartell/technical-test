using System.ComponentModel.DataAnnotations;
using BiographicalDetails.Domain;

namespace BiographicalDetails.EntityModels;

public class BiographicalDataEntity
{
	[Key]
	public required int Id { get; set; }

	[Required]
	[StringLength(100)]
	public required string FirstName { get; set; }

	[Required]
	[StringLength(100)]
	public required string LastName { get; set; }

	[Required]
	[StringLength(100)]
	public required string Email { get; set; }

	[StringLength(50)]
	public string? PreferredPronouns { get; set; }

	[Required]
	public required LevelOfStudy LevelOfStudy { get; set; }

	[Required]
	public required ImmigrationStatus ImmigrationStatus { get; set; }

	[StringLength(20)]
	public string? SocialInsuranceNumber { get; set; }

	[StringLength(20)]
	public string? UniqueClientIdentifier { get; set; }
}