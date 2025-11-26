namespace BiographicalDetails.Domain;

public record BiographicalData
{
	public int Id { get; set; } = 0;
	public required string FirstName { get; set; }
	public required string LastName { get; set; }
	public required string Email { get; set; }
	public string? PreferredPronouns { get; set; }
	public LevelOfStudy LevelOfStudy { get; set; }
	public ImmigrationStatus ImmigrationStatus { get; set; }
	public string? SocialInsuranceNumber { get; set; }
	public string? UniqueClientIdentifier { get; set; }
}