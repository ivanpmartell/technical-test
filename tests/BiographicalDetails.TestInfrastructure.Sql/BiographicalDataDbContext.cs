// Use this project to create migrations for the Test Sql Database

using BiographicalDetails.Domain;
using BiographicalDetails.Infrastructure.Sql.Contexts.Extensions;
using BiographicalDetails.Infrastructure.Sql.Contexts.Loggers;
using Microsoft.EntityFrameworkCore;

namespace BiographicalDetails.Infrastructure.Sql.Contexts;

public class BiographicalDataTestDbContext : DbContext
{
	public BiographicalDataTestDbContext()
		: base() { }

	public BiographicalDataTestDbContext(DbContextOptions<BiographicalDataTestDbContext> options) : base(options) { }

	public DbSet<BiographicalDataEntity> BiographicalDatas { get; set; }

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		if (!optionsBuilder.IsConfigured)
		{
			optionsBuilder.UseSqlServer(BiographicalDataContextExtensions.DefaultConnectionString("BiographicalDetails_Tests"));
			optionsBuilder.LogTo(BiographicalDataTestLogger.WriteLine,
				  [Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.CommandExecuting]);
		}
	}
	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);
	}
}