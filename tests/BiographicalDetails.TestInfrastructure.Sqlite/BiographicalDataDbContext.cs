using BiographicalDetails.Infrastructure.Sqlite.Contexts;
using BiographicalDetails.Infrastructure.Sqlite.Contexts.Extensions;
using BiographicalDetails.TestInfrastructure.Sqlite.Loggers;
using Microsoft.EntityFrameworkCore;

namespace BiographicalDetails.TestInfrastructure.Sqlite;

public class BiographicalDataTestDbContext : BiographicalDataDbContext
{
	public BiographicalDataTestDbContext()
		: base() { }

	public BiographicalDataTestDbContext(DbContextOptions<BiographicalDataDbContext> options) : base(options) { }

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		if (!optionsBuilder.IsConfigured)
		{
			optionsBuilder.UseSqlite(BiographicalDataContextExtensions.DefaultConnectionString("BiographicalDetails_Tests"));
			optionsBuilder.LogTo(BiographicalDataTestLogger.WriteLine,
				  [Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.CommandExecuting]);
		}
	}
	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);
	}
}