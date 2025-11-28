using BiographicalDetails.Domain;
using BiographicalDetails.EntityModels;
using BiographicalDetails.EntityModels.Abstractions;
using BiographicalDetails.EntityModels.Mappers;
using BiographicalDetails.Infrastructure.Sqlite;
using BiographicalDetails.Infrastructure.Sqlite.Contexts;
using BiographicalDetails.Infrastructure.Sqlite.Contexts.Extensions;
using BiographicalDetails.Infrastructure.Sqlite.Contexts.Loggers;
using Microsoft.EntityFrameworkCore;

namespace BiographicalDetails.Integration.Tests;

public class SqliteDatabaseFixture : IDisposable
{
	internal BiographicalDataDbContext context;
	internal SqliteBiographicalDataRepository sqLiteRepository;
	internal IBiographicalDataMapper mapper;

	public SqliteDatabaseFixture()
	{
		var dbName = $"BiographicalDetails_Tests";

		var options = new DbContextOptionsBuilder<BiographicalDataDbContext>()
			.UseSqlite(BiographicalDataContextExtensions.DefaultConnectionString(dbName))
			.LogTo(BiographicalDataLogger.WriteLine,
			  [Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.CommandExecuting])
			.Options;

		context = new BiographicalDataDbContext(options);
		mapper = new BiographicalDataEntityMapper();
		sqLiteRepository = new SqliteBiographicalDataRepository(context, mapper);
	}

	public void Dispose()
	{
		context.Dispose();
	}
}

public class SqliteBiographicalDataRepositoryTests : IClassFixture<SqliteDatabaseFixture>, IDisposable
{
	private SqliteDatabaseFixture _dbFixture;

	public SqliteBiographicalDataRepositoryTests(SqliteDatabaseFixture dbFixture)
	{
		_dbFixture = dbFixture;
	}

	public void Dispose()
	{
		_dbFixture.context.Users.RemoveRange(_dbFixture.context.Users);
		_dbFixture.context.UserPronouns.RemoveRange(_dbFixture.context.UserPronouns);
		_dbFixture.context.UserSins.RemoveRange(_dbFixture.context.UserSins);
		_dbFixture.context.UserUcis.RemoveRange(_dbFixture.context.UserUcis);
		_dbFixture.context.SaveChanges();
	}

	[Fact]
	public async Task AddAsync_BiographicalData_ShouldAddToDatabase()
	{
		//Arrange
		var biographicalData = new BiographicalData
		{
			Id = 0,
			FirstName = "Ivan",
			LastName = "Perez",
			Email = "ivan@test.com",
			PreferredPronouns = "",
			LevelOfStudy = LevelOfStudy.SomeCollege,
			ImmigrationStatus = ImmigrationStatus.Visitor,
			SocialInsuranceNumber = null,
			UniqueClientIdentifier = "0000-0000"
		};

		//Act

		var addedBiographicalData = await _dbFixture.sqLiteRepository.AddAsync(biographicalData);

		//Assert
		Assert.NotNull(addedBiographicalData);
		var userInDb = await _dbFixture.context.FindAsync<UserEntity>(addedBiographicalData.Id);
		Assert.NotNull(userInDb);
		Assert.Equal(userInDb.Id, addedBiographicalData.Id);

		var pronounsInDb = await _dbFixture.context.UserPronouns.FirstOrDefaultAsync();
		Assert.NotNull(pronounsInDb);
		Assert.Equal(pronounsInDb.PreferredPronouns, addedBiographicalData.PreferredPronouns);

		var sinInDb = await _dbFixture.context.UserSins.FirstOrDefaultAsync();
		Assert.Null(sinInDb);

		var uciInDb = await _dbFixture.context.UserUcis.FirstOrDefaultAsync();
		Assert.NotNull(uciInDb);
		Assert.Equal(uciInDb.UniqueClientIdentifier, addedBiographicalData.UniqueClientIdentifier);
	}

	[Fact]
	public async Task GetAsync_WithNonExistingData_ShouldReturnNull()
	{
		//Arrange
		var biographicalDataId = 999;

		//Act
		var biographicalData = await _dbFixture.sqLiteRepository.GetAsync(biographicalDataId);

		//Assert
		Assert.Null(biographicalData);
	}

	[Fact]
	public async Task GetAllAsync_WithNonExistingData_ShouldReturnEmptyCollection()
	{
		//Arrange

		//Act
		var biographicalData = await _dbFixture.sqLiteRepository.GetAllAsync();

		//Assert
		Assert.NotNull(biographicalData);
		Assert.Empty(biographicalData);
	}

	[Fact]
	public async Task GetAllAsync_WithExistingData_ShouldReturnCollection()
	{
		//Arrange
		var biographicalData = new BiographicalData
		{
			Id = 0,
			FirstName = "Ivan",
			LastName = "Perez",
			Email = "ivan@test.com",
			PreferredPronouns = "He/Him",
			LevelOfStudy = LevelOfStudy.SomeCollege,
			ImmigrationStatus = ImmigrationStatus.Visitor,
			SocialInsuranceNumber = null,
			UniqueClientIdentifier = "0000-0000"
		};

		var anotherBiographicalData = new BiographicalData
		{
			Id = 0,
			FirstName = "Juan",
			LastName = "Perez",
			Email = "juan@test2.com",
			PreferredPronouns = "He/Him",
			LevelOfStudy = LevelOfStudy.HighSchoolDiploma,
			ImmigrationStatus = ImmigrationStatus.Visitor,
			SocialInsuranceNumber = null,
			UniqueClientIdentifier = "0000-0000"
		};

		var addedBiographicalData = await _dbFixture.sqLiteRepository.AddAsync(biographicalData);
		var addedAnotherBiographicalData = await _dbFixture.sqLiteRepository.AddAsync(anotherBiographicalData);
		Assert.NotNull(addedBiographicalData);
		Assert.NotNull(addedAnotherBiographicalData);
		biographicalData.Id = addedBiographicalData.Id;
		anotherBiographicalData.Id = addedAnotherBiographicalData.Id;

		//Act
		var retrievedData = await _dbFixture.sqLiteRepository.GetAllAsync();

		//Assert
		Assert.NotNull(retrievedData);
		Assert.Collection(retrievedData,
			item => Assert.Equal(biographicalData, item),
			item => Assert.Equal(anotherBiographicalData, item)
		);
	}

	[Fact]
	public async Task DeleteAsync_WithExistingData_ShouldReturnTrue()
	{
		//Arrange
		var biographicalData = new BiographicalData
		{
			Id = 0,
			FirstName = "Ivan",
			LastName = "Perez",
			Email = "ivan@test.com",
			PreferredPronouns = "He/Him",
			LevelOfStudy = LevelOfStudy.SomeCollege,
			ImmigrationStatus = ImmigrationStatus.Visitor,
			SocialInsuranceNumber = null,
			UniqueClientIdentifier = "0000-0000"
		};

		var addedBiographicalData = await _dbFixture.sqLiteRepository.AddAsync(biographicalData);
		Assert.NotNull(addedBiographicalData);
		biographicalData.Id = addedBiographicalData.Id;

		//Act
		var deleted = await _dbFixture.sqLiteRepository.DeleteAsync(biographicalData.Id);

		//Assert
		Assert.True(deleted);
		Assert.Null(await _dbFixture.sqLiteRepository.GetAsync(biographicalData.Id));
	}

	[Fact]
	public async Task DeleteAsync_WithNonExistingData_ShouldReturnFalse()
	{
		// Arrange
		var biographicalDataId = 999;

		// Act
		var deleted = await _dbFixture.sqLiteRepository.DeleteAsync(biographicalDataId);

		// Assert
		Assert.False(deleted);
	}

	[Fact]
	public async Task UpdateAsync_WithExistingData_ShouldReturnTrue()
	{
		//Arrange
		var biographicalData = new BiographicalData
		{
			Id = 0,
			FirstName = "Ivan",
			LastName = "Perez",
			Email = "ivan@test.com",
			PreferredPronouns = "He/Him",
			LevelOfStudy = LevelOfStudy.SomeCollege,
			ImmigrationStatus = ImmigrationStatus.Visitor,
			SocialInsuranceNumber = null,
			UniqueClientIdentifier = "0000-0000"
		};

		var addedBiographicalData = await _dbFixture.sqLiteRepository.AddAsync(biographicalData);
		Assert.NotNull(addedBiographicalData);
		var updatedBiographicalData = new BiographicalData
		{
			Id = addedBiographicalData.Id,
			FirstName = "Ivan",
			LastName = "Yay",
			Email = "perez@test2.com",
			PreferredPronouns = "He/Him",
			LevelOfStudy = LevelOfStudy.SomeCollege,
			ImmigrationStatus = ImmigrationStatus.Visitor,
			SocialInsuranceNumber = null,
			UniqueClientIdentifier = "0000-0000"
		};

		//Act
		var updated = await _dbFixture.sqLiteRepository.UpdateAsync(updatedBiographicalData);

		//Assert
		Assert.True(updated);
		Assert.Equal(updatedBiographicalData, await _dbFixture.sqLiteRepository.GetAsync(addedBiographicalData.Id));
	}

	[Fact]
	public async Task UpdateAsync_WithNonExistingData_ShouldReturnFalse()
	{
		// Arrange
		var biographicalDataId = 999;
		var biographicalData = new BiographicalData
		{
			Id = biographicalDataId,
			FirstName = "Ivan",
			LastName = "Perez",
			Email = "ivan@test.com",
			PreferredPronouns = "He/Him",
			LevelOfStudy = LevelOfStudy.SomeCollege,
			ImmigrationStatus = ImmigrationStatus.Visitor,
			SocialInsuranceNumber = null,
			UniqueClientIdentifier = "0000-0000"
		};

		// Act
		var updated = await _dbFixture.sqLiteRepository.UpdateAsync(biographicalData);

		// Assert
		Assert.False(updated);
	}
}
