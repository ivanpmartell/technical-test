using BiographicalDetails.Domain;
using BiographicalDetails.EntityModels;
using BiographicalDetails.EntityModels.Abstractions;
using BiographicalDetails.EntityModels.Mappers;
using BiographicalDetails.Infrastructure.InMemory;
using BiographicalDetails.Infrastructure.InMemory.Contexts;
using Microsoft.EntityFrameworkCore;

namespace BiographicalDetails.Integration.Tests;

public class InMemoryDatabaseFixture : IDisposable
{
	internal BiographicalDataDbContext context;
	internal InMemoryBiographicalDataRepository inMemoryRepository;
	internal IBiographicalDataMapper mapper;

	public InMemoryDatabaseFixture()
	{
		var options = new DbContextOptionsBuilder<BiographicalDataDbContext>()
			.UseInMemoryDatabase(databaseName: $"BiographicalDetailsTestInMemoryDb_{Guid.NewGuid()}")
			.Options;

		context = new BiographicalDataDbContext(options);
		mapper = new BiographicalDataMapper();
		inMemoryRepository = new InMemoryBiographicalDataRepository(context, mapper);
	}

	public void Dispose()
	{
		context.Database.EnsureDeleted();
		context.Dispose();
	}
}

public class InMemoryBiographicalDataRepositoryTests : IClassFixture<InMemoryDatabaseFixture>, IDisposable
{
	private InMemoryDatabaseFixture _dbFixture;

	public InMemoryBiographicalDataRepositoryTests(InMemoryDatabaseFixture dbFixture)
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
	public void DatabaseConnectTest()
	{
		Assert.True(_dbFixture.context.Database.CanConnect());
	}

	[Fact]
	public async Task AddAsync_BiographicalData_ShouldAddToDatabase()
	{
		//Arrange
		var biographicalDataId = 3;
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

		//Act

		await _dbFixture.inMemoryRepository.AddAsync(biographicalData);

		//Assert
		var userInDb = await _dbFixture.context.FindAsync<UserEntity>(biographicalDataId);
		Assert.NotNull(userInDb);
		Assert.Equal(biographicalDataId, userInDb.Id);
		Assert.Equal(biographicalData, await _dbFixture.inMemoryRepository.GetAsync(biographicalDataId));
	}

	[Fact]
	public async Task GetAsync_WithNonExistingData_ShouldReturnNull()
	{
		//Arrange
		var biographicalDataId = 999;

		//Act
		var biographicalData = await _dbFixture.inMemoryRepository.GetAsync(biographicalDataId);

		//Assert
		Assert.Null(biographicalData);
	}

	[Fact]
	public async Task GetAllAsync_WithNonExistingData_ShouldReturnEmptyCollection()
	{
		//Arrange

		//Act
		var biographicalData = await _dbFixture.inMemoryRepository.GetAllAsync();

		//Assert
		Assert.NotNull(biographicalData);
		Assert.Empty(biographicalData);
	}

	[Fact]
	public async Task GetAllAsync_WithExistingData_ShouldReturnCollection()
	{
		//Arrange
		var biographicalDataId = 1;
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

		await _dbFixture.inMemoryRepository.AddAsync(biographicalData);

		//Act
		var retrievedData = await _dbFixture.inMemoryRepository.GetAllAsync();

		//Assert
		Assert.NotNull(retrievedData);
		Assert.Collection(retrievedData,
			item => Assert.Equal(biographicalData, item)
		);
	}

	[Fact]
	public async Task DeleteAsync_WithExistingData_ShouldReturnTrue()
	{
		//Arrange
		var biographicalDataId = 1;
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

		await _dbFixture.inMemoryRepository.AddAsync(biographicalData);

		//Act
		var deleted = await _dbFixture.inMemoryRepository.DeleteAsync(biographicalDataId);

		//Assert
		Assert.True(deleted);
		Assert.Null(await _dbFixture.inMemoryRepository.GetAsync(biographicalDataId));
	}

	[Fact]
	public async Task DeleteAsync_WithNonExistingData_ShouldReturnFalse()
	{
		// Arrange
		var biographicalDataId = 999;

		// Act
		var deleted = await _dbFixture.inMemoryRepository.DeleteAsync(biographicalDataId);

		// Assert
		Assert.False(deleted);
	}

	[Fact]
	public async Task UpdateAsync_WithExistingData_ShouldReturnTrue()
	{
		//Arrange
		var biographicalDataId = 1;
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

		var updatedBiographicalData = new BiographicalData
		{
			Id = biographicalDataId,
			FirstName = "Ivan",
			LastName = "Yay",
			Email = "perez@test2.com",
			PreferredPronouns = "He/Him",
			LevelOfStudy = LevelOfStudy.SomeCollege,
			ImmigrationStatus = ImmigrationStatus.Visitor,
			SocialInsuranceNumber = null,
			UniqueClientIdentifier = "0000-0000"
		};

		await _dbFixture.inMemoryRepository.AddAsync(biographicalData);

		//Act
		var updated = await _dbFixture.inMemoryRepository.UpdateAsync(updatedBiographicalData);

		//Assert
		Assert.True(updated);
		Assert.Equal(updatedBiographicalData, await _dbFixture.inMemoryRepository.GetAsync(biographicalDataId));
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
		var updated = await _dbFixture.inMemoryRepository.UpdateAsync(biographicalData);

		// Assert
		Assert.False(updated);
	}
}
