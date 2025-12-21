using System.ComponentModel.DataAnnotations;

namespace BiographicalDetails.EntityModels;

public class UserPronounEntity
{
	[Key]
	public int Id { get; set; }

	[Required]
	public int UserId { get; set; }

	[Required]
	public required string PreferredPronouns { get; set; }
}
