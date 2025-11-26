using Microsoft.EntityFrameworkCore;
using BiographicalDetails.EntityModels;

namespace BiographicalDetails.Infrastructure.InMemory.Contexts;

public class BiographicalDataDbContext(DbContextOptions<BiographicalDataDbContext> options) : DbContext(options)
{
	public virtual DbSet<UserEntity> Users { get; set; }
	public virtual DbSet<UserPronounEntity> UserPronouns { get; set; }
	public virtual DbSet<UserSinEntity> UserSins { get; set; }
	public virtual DbSet<UserUciEntity> UserUcis { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);
	}
}
