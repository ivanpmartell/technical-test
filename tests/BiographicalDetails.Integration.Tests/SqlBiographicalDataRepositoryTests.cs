using BiographicalDetails.Domain;
using BiographicalDetails.EntityModels.Abstractions;
using BiographicalDetails.EntityModels.Mappers;
using BiographicalDetails.Infrastructure.Sql;
using BiographicalDetails.Infrastructure.Sql.Contexts;
using BiographicalDetails.Infrastructure.Sql.Contexts.Extensions;
using BiographicalDetails.TestInfrastructure.Sql.Loggers;
using Microsoft.EntityFrameworkCore;


namespace BiographicalDetails.Integration.Tests;

public class SqlDatabaseFixture : IDisposable
{
	internal SqlBiographicalDataRepository sqlRepository;
	internal BiographicalDataDbContext context;
	internal IBiographicalDataMapper mapper;

	public SqlDatabaseFixture()
	{
		var dbName = $"BiographicalDetails_Tests";

		var options = new DbContextOptionsBuilder<BiographicalDataDbContext>()
			.UseSqlServer(BiographicalDataContextExtensions.DefaultConnectionString(dbName))
			.LogTo(BiographicalDataTestLogger.WriteLine,
				  [Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.CommandExecuting])
			.Options;

		context = new BiographicalDataDbContext(options);
		mapper = new BiographicalDataEntityMapper();
		sqlRepository = new SqlBiographicalDataRepository(context, mapper);
	}

	public void Dispose()
	{
		context.Dispose();
	}
}

public class SqlBiographicalDataRepositoryTests : IClassFixture<SqlDatabaseFixture>, IDisposable
{
	private SqlDatabaseFixture _dbFixture;

	public SqlBiographicalDataRepositoryTests(SqlDatabaseFixture dbFixture)
	{
		_dbFixture = dbFixture;
	}

	public void Dispose()
	{
		_dbFixture.context.BiographicalDatas.RemoveRange(_dbFixture.context.BiographicalDatas);
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

		//Act

		var addedBiographicalData = await _dbFixture.sqlRepository.AddAsync(biographicalData);

		//Assert
		Assert.NotNull(addedBiographicalData);
		var dataInDb = await _dbFixture.context.FindAsync<BiographicalDataEntity>(addedBiographicalData.Id);
		Assert.NotNull(dataInDb);
		Assert.Equal(dataInDb.Id, addedBiographicalData.Id);
	}

	[Fact]
	public async Task GetAsync_WithNonExistingData_ShouldReturnNull()
	{
		//Arrange
		var biographicalDataId = 999;

		//Act
		var biographicalData = await _dbFixture.sqlRepository.GetAsync(biographicalDataId);

		//Assert
		Assert.Null(biographicalData);
	}

	[Fact]
	public async Task GetAllAsync_WithNonExistingData_ShouldReturnEmptyCollection()
	{
		//Arrange

		//Act
		var biographicalData = await _dbFixture.sqlRepository.GetAllAsync();

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

		var addedBiographicalData = await _dbFixture.sqlRepository.AddAsync(biographicalData);
		var addedAnotherBiographicalData = await _dbFixture.sqlRepository.AddAsync(anotherBiographicalData);
		Assert.NotNull(addedBiographicalData);
		Assert.NotNull(addedAnotherBiographicalData);
		biographicalData.Id = addedBiographicalData.Id;
		anotherBiographicalData.Id = addedAnotherBiographicalData.Id;

		//Act
		var retrievedData = await _dbFixture.sqlRepository.GetAllAsync();

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

		var addedBiographicalData = await _dbFixture.sqlRepository.AddAsync(biographicalData);
		Assert.NotNull(addedBiographicalData);
		biographicalData.Id = addedBiographicalData.Id;

		//Act
		var deleted = await _dbFixture.sqlRepository.DeleteAsync(biographicalData.Id);

		//Assert
		Assert.True(deleted);
		Assert.Null(await _dbFixture.sqlRepository.GetAsync(biographicalData.Id));
	}

	[Fact]
	public async Task DeleteAsync_WithNonExistingData_ShouldReturnFalse()
	{
		// Arrange
		var biographicalDataId = 999;

		// Act
		var deleted = await _dbFixture.sqlRepository.DeleteAsync(biographicalDataId);

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

		var addedBiographicalData = await _dbFixture.sqlRepository.AddAsync(biographicalData);
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
		var updated = await _dbFixture.sqlRepository.UpdateAsync(updatedBiographicalData);

		//Assert
		Assert.True(updated);
		Assert.Equal(updatedBiographicalData, await _dbFixture.sqlRepository.GetAsync(addedBiographicalData.Id));
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
		var updated = await _dbFixture.sqlRepository.UpdateAsync(biographicalData);

		// Assert
		Assert.False(updated);
	}
}
