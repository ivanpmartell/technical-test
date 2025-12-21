using Microsoft.EntityFrameworkCore;
using BiographicalDetails.EntityModels;
using BiographicalDetails.Infrastructure.Sqlite.Contexts.Extensions;
using BiographicalDetails.Helpers;

namespace BiographicalDetails.Infrastructure.Sqlite.Contexts;

public class BiographicalDataDbContext: DbContext
{
	public BiographicalDataDbContext()
		: base() { }

	public BiographicalDataDbContext(DbContextOptions<BiographicalDataDbContext> options) : base(options) { }

	public DbSet<UserEntity> Users { get; set; }
	public DbSet<UserPronounEntity> UserPronouns { get; set; }
	public DbSet<UserSinEntity> UserSins { get; set; }
	public DbSet<UserUciEntity> UserUcis { get; set; }

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		if (!optionsBuilder.IsConfigured)
		{
			var logger = new BiographicalDataLogger
			{
				FolderName = "sqlite-logs"
			};

			optionsBuilder.UseSqlite(BiographicalDataContextExtensions.DefaultConnectionString("BiographicalDetails"));
			optionsBuilder.LogTo(logger.WriteLine,
				  [Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.CommandExecuting]);
		}
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<UserEntity>()
			.HasOne<UserPronounEntity>()
			.WithOne()
			.HasForeignKey<UserPronounEntity>(e => e.UserId)
			.IsRequired();

		modelBuilder.Entity<UserEntity>()
			.HasOne<UserSinEntity>()
			.WithOne()
			.HasForeignKey<UserSinEntity>(e => e.UserId)
			.IsRequired();

		modelBuilder.Entity<UserEntity>()
			.HasOne<UserUciEntity>()
			.WithOne()
			.HasForeignKey<UserUciEntity>(e => e.UserId)
			.IsRequired();

		base.OnModelCreating(modelBuilder);
	}
}
