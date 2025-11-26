using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BiographicalDetails.EntityModels;

public class UserUciEntity
{
	[Key]
	public int Id { get; set; }

	[Required]
	[ForeignKey(nameof(UserEntity))]
	public int UserId { get; set; }

	[Required]
	public required string UniqueClientIdentifier { get; set; }
}
