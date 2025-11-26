using BiographicalDetails.Domain;
using BiographicalDetails.Infrastructure.Sql.Contexts.Extensions;
using BiographicalDetails.Infrastructure.Sql.Contexts.Loggers;
using Microsoft.EntityFrameworkCore;

namespace BiographicalDetails.Infrastructure.Sql.Contexts;

public class BiographicalDataDbContext : DbContext
{
	public BiographicalDataDbContext()
		: base() { }

	public BiographicalDataDbContext(DbContextOptions<BiographicalDataDbContext> options) : base(options) { }

	public DbSet<BiographicalDataEntity> BiographicalDatas { get; set; }

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		if (!optionsBuilder.IsConfigured)
		{
			optionsBuilder.UseSqlServer(BiographicalDataContextExtensions.DefaultConnectionString("BiographicalDetails"));
			optionsBuilder.LogTo(BiographicalDataLogger.WriteLine,
				  [Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.CommandExecuting]);
		}
	}
	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);

		modelBuilder.Entity<BiographicalDataEntity>().HasData(
			new BiographicalDataEntity
			{
				Id = 1,
				FirstName = "Ivan",
				LastName = "Perez",
				Email = "ivan@test.com",
				PreferredPronouns = "He/Him",
				LevelOfStudy = LevelOfStudy.HighSchoolDiploma,
				ImmigrationStatus = ImmigrationStatus.Visitor,
				SocialInsuranceNumber = null,
				UniqueClientIdentifier = "0000-0000"
			},
			new BiographicalDataEntity
			{
				Id = 2,
				FirstName = "Juan",
				LastName = "Perez",
				Email = "juan@yay.com",
				PreferredPronouns = "They/Them",
				LevelOfStudy = LevelOfStudy.SomeCollege,
				ImmigrationStatus = ImmigrationStatus.CanadianCitizen,
				SocialInsuranceNumber = "000-000-000",
				UniqueClientIdentifier = null
			}
		);
	}
}