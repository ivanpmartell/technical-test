using System.ComponentModel.DataAnnotations;

namespace BiographicalDetails.EntityModels;

public class UserUciEntity
{
	[Key]
	public int Id { get; set; }

	[Required]
	public int UserId { get; set; }

	[Required]
	public required string UniqueClientIdentifier { get; set; }
}
