using System.ComponentModel.DataAnnotations;
using BiographicalDetails.Domain;

namespace BiographicalDetails.EntityModels;

public class UserEntity
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

	[Required]
	public required LevelOfStudy LevelOfStudy { get; set; }

	[Required]
	public required ImmigrationStatus ImmigrationStatus { get; set; }
}
