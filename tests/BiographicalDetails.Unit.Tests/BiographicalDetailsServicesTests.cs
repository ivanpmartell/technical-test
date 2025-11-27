using System.Collections.ObjectModel;
using BiographicalDetails.Application.Errors;
using BiographicalDetails.Application.Services;
using BiographicalDetails.Application.Validators;
using BiographicalDetails.Domain;
using BiographicalDetails.Domain.Abstractions;
using Moq;

namespace BiographicalDetails.Unit.Tests;

public class BiographicalDetailsServicesTests
{
	BiographicalDetailsService _biographicalDetailsService;
	Mock<IBiographicalDataRepository> _biographicalDetailsRepositoryMock;
	IBiographicalDataValidator _biographicalDetailsValidator;

	public BiographicalDetailsServicesTests()
	{
		_biographicalDetailsRepositoryMock = new Mock<IBiographicalDataRepository>();
		_biographicalDetailsValidator = new BiographicalDataValidator(new SINValidator(), new UCIValidator());
		_biographicalDetailsService = new BiographicalDetailsService(_biographicalDetailsRepositoryMock.Object, _biographicalDetailsValidator);
	}

	#region SAVE

	[Fact]
	public async Task SaveBiographicalInfo_WithValidData_ShouldCreateBiographicalData()
	{
		//Arrange
		var submission = new BiographicalData
		{
			FirstName = "Ivan",
			LastName = "Perez",
			Email = "ivan@test.com",
			PreferredPronouns = "He/Him",
			LevelOfStudy = LevelOfStudy.SomeCollege,
			ImmigrationStatus = ImmigrationStatus.Visitor,
			SocialInsuranceNumber = null,
			UniqueClientIdentifier = "0000-0000"
		};

		_biographicalDetailsRepositoryMock.Setup(repo => repo.AddAsync(submission)).ReturnsAsync(submission);

		//Act
		var result = await _biographicalDetailsService.SaveBiographicalInfoAsync(submission);

		//Assert
		Assert.NotNull(result);
		Assert.Equal(submission.FirstName, result.FirstName);
		Assert.Equal(submission.LastName, result.LastName);
		Assert.Equal(submission.Email, result.Email);
		Assert.Equal(submission.PreferredPronouns, result.PreferredPronouns);
		Assert.Equal(submission.LevelOfStudy, result.LevelOfStudy);
		Assert.Equal(submission.ImmigrationStatus, result.ImmigrationStatus);
		Assert.Equal(submission.SocialInsuranceNumber, result.SocialInsuranceNumber);
		Assert.Equal(submission.UniqueClientIdentifier, result.UniqueClientIdentifier);
	}

	[Theory]//TODO class data for sin requiring and uci requiring
	[InlineData(ImmigrationStatus.CanadianCitizen)]
	[InlineData(ImmigrationStatus.PermanentResident)]
	[InlineData(ImmigrationStatus.TemporaryForeignWorker)]
	[InlineData(ImmigrationStatus.ProtectedPerson)]
	[InlineData(ImmigrationStatus.Indigenous)]
	public async Task SaveBiographicalInfo_WhenImmigrationStatusChosenRequiringSIN_ShouldThrowException(ImmigrationStatus status)
	{
		//Arrange
		var submission = new BiographicalData
		{
			FirstName = "Ivan",
			LastName = "Perez",
			Email = "ivan@test.com",
			PreferredPronouns = "He/Him",
			LevelOfStudy = LevelOfStudy.SomeCollege,
			ImmigrationStatus = status,
			SocialInsuranceNumber = null,
			UniqueClientIdentifier = "0000-0000"
		};

		//Act
		var result = async () => await _biographicalDetailsService.SaveBiographicalInfoAsync(submission);

		//Assert
		var exception = await Assert.ThrowsAsync<SINException>(result);
		Assert.Equal(BiographicalDetailsErrors.RequiredSIN_Missing, exception.Message);
	}

	[Theory]
	[InlineData(ImmigrationStatus.PermanentResident)]
	[InlineData(ImmigrationStatus.TemporaryForeignWorker)]
	[InlineData(ImmigrationStatus.InternationalStudent)]
	[InlineData(ImmigrationStatus.ProtectedPerson)]
	[InlineData(ImmigrationStatus.Visitor)]
	public async Task SaveBiographicalInfo_WhenImmigrationStatusChosenRequiringUCI_ShouldThrowException(ImmigrationStatus status)
	{
		//Arrange
		var submission = new BiographicalData
		{
			FirstName = "Ivan",
			LastName = "Perez",
			Email = "ivan@test.com",
			PreferredPronouns = "He/Him",
			LevelOfStudy = LevelOfStudy.SomeCollege,
			ImmigrationStatus = status,
			SocialInsuranceNumber = "046-454-286",
			UniqueClientIdentifier = null
		};

		//Act
		var result = async () => await _biographicalDetailsService.SaveBiographicalInfoAsync(submission);

		//Assert
		var exception = await Assert.ThrowsAsync<UCIException>(result);
		Assert.Equal(BiographicalDetailsErrors.RequiredUCI_Missing, exception.Message);
	}

	[Fact]
	public async Task SaveBiographicalInfo_WithValidData_ShouldAddBiographicalData()
	{
		//Arrange
		var submission = new BiographicalData
		{
			FirstName = "Ivan",
			LastName = "Perez",
			Email = "ivan@test.com",
			PreferredPronouns = "He/Him",
			LevelOfStudy = LevelOfStudy.SomeCollege,
			ImmigrationStatus = ImmigrationStatus.Visitor,
			SocialInsuranceNumber = null,
			UniqueClientIdentifier = "0000-0000"
		};

		_biographicalDetailsRepositoryMock.Setup(repo => repo.AddAsync(submission)).ReturnsAsync(submission);

		//Act
		var result = await _biographicalDetailsService.SaveBiographicalInfoAsync(submission);

		//Assert
		_biographicalDetailsRepositoryMock.Verify(repo => repo.AddAsync(It.Is<BiographicalData>(bd =>
			bd.FirstName == submission.FirstName &&
			bd.LastName == submission.LastName &&
			bd.Email== submission.Email &&
			bd.PreferredPronouns == submission.PreferredPronouns &&
			bd.LevelOfStudy == submission.LevelOfStudy &&
			bd.ImmigrationStatus == submission.ImmigrationStatus &&
			bd.SocialInsuranceNumber == submission.SocialInsuranceNumber &&
			bd.UniqueClientIdentifier == submission.UniqueClientIdentifier

		)), Times.Once);
	}

	[Fact]
	public async Task SaveBiographicalInfo_WhenExceptionThrown_ShouldNotAddBiographicalData()
	{
		//Arrange
		var submission = new BiographicalData
		{
			FirstName = "Ivan",
			LastName = "Perez",
			Email = "ivan@test.com",
			PreferredPronouns = "He/Him",
			LevelOfStudy = LevelOfStudy.SomeCollege,
			ImmigrationStatus = ImmigrationStatus.Visitor,
			SocialInsuranceNumber = null,
			UniqueClientIdentifier = null
		};

		//Act
		var result = async () => await _biographicalDetailsService.SaveBiographicalInfoAsync(submission);

		//Assert
		var _ = await Assert.ThrowsAsync<UCIException>(result);
		_biographicalDetailsRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<BiographicalData>()), Times.Never);
	}

#endregion ADD

	#region GET

	[Fact]
	public async Task GetBiographicalInfo_ForExistingBiographicalData_ShouldReturnBiographicalData()
	{
		//Arrange
		var biographicalDataId = 1;
		var existingBiographicalData = new BiographicalData
		{
			FirstName = "Ivan",
			LastName = "Perez",
			Email = "ivan@test.com",
			PreferredPronouns = "He/Him",
			LevelOfStudy = LevelOfStudy.SomeCollege,
			ImmigrationStatus = ImmigrationStatus.Visitor,
			SocialInsuranceNumber = null,
			UniqueClientIdentifier = "0000-0000"
		};

		_biographicalDetailsRepositoryMock.Setup(repo => repo.GetAsync(biographicalDataId)).ReturnsAsync(existingBiographicalData);

		//Act
		var result = await _biographicalDetailsService.GetBiographicalInfoAsync(biographicalDataId);

		//Assert
		Assert.NotNull(result);
		Assert.Equal(existingBiographicalData, result);
		_biographicalDetailsRepositoryMock.Verify(repo => repo.GetAsync(biographicalDataId), Times.Once);
	}

	[Fact]
	public async Task GetBiographicalInfo_ForNonExistentData_ShouldReturnNull()
	{
		//Arrange
		var biographicalDataId = 9999;
		_biographicalDetailsRepositoryMock.Setup(repo => repo.GetAsync(biographicalDataId)).ReturnsAsync((BiographicalData?)null);

		//Act
		var result = await _biographicalDetailsService.GetBiographicalInfoAsync(biographicalDataId);

		//Assert
		Assert.Null(result);
		_biographicalDetailsRepositoryMock.Verify(repo => repo.GetAsync(biographicalDataId), Times.Once);
	}

#endregion GET

	#region GETALL

	[Fact]
	public async Task GetAllBiographicalInfos_ForSingleData_ShouldReturnCollectionOfBiographicalInfo()
	{
		//Arrange
		var existingBiographicalData = new BiographicalData
		{
			FirstName = "Ivan",
			LastName = "Perez",
			Email = "ivan@test.com",
			PreferredPronouns = "He/Him",
			LevelOfStudy = LevelOfStudy.SomeCollege,
			ImmigrationStatus = ImmigrationStatus.Visitor,
			SocialInsuranceNumber = null,
			UniqueClientIdentifier = "0000-0000"
		};

		var existingBiographicalDataCollection = new Collection<BiographicalData>{
			existingBiographicalData
		};

		_biographicalDetailsRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(existingBiographicalDataCollection);

		//Act
		var result = await _biographicalDetailsService.GetAllBiographicalInfosAsync();

		//Assert
		Assert.NotNull(result);
		var item = Assert.Single(result);
		Assert.Equal(existingBiographicalData, item);
		_biographicalDetailsRepositoryMock.Verify(repo => repo.GetAllAsync(), Times.Once);
	}

	[Fact]
	public async Task GetAllBiographicalInfos_ForMultipleData_ShouldReturnCollectionOfBiographicalInfo()
	{
		//Arrange
		var existingBiographicalData = new BiographicalData
		{
			FirstName = "Ivan",
			LastName = "Perez",
			Email = "ivan@test.com",
			PreferredPronouns = "He/Him",
			LevelOfStudy = LevelOfStudy.SomeCollege,
			ImmigrationStatus = ImmigrationStatus.Visitor,
			SocialInsuranceNumber = null,
			UniqueClientIdentifier = "0000-0000"
		};

		var anotherExistingBiographicalData = new BiographicalData
		{
			FirstName = "Hello",
			LastName = "World",
			Email = "hello@test.com",
			PreferredPronouns = "He/Him",
			LevelOfStudy = LevelOfStudy.SomeCollege,
			ImmigrationStatus = ImmigrationStatus.Visitor,
			SocialInsuranceNumber = null,
			UniqueClientIdentifier = "0000-0000"
		};

		var existingBiographicalDataCollection = new Collection<BiographicalData>{
			existingBiographicalData,
			anotherExistingBiographicalData
		};

		_biographicalDetailsRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(existingBiographicalDataCollection);

		//Act
		var result = await _biographicalDetailsService.GetAllBiographicalInfosAsync();

		//Assert
		Assert.NotNull(result);
		Assert.Collection(result,
			item => Assert.Equal(existingBiographicalData, item),
			item => Assert.Equal(anotherExistingBiographicalData, item)
		);
		_biographicalDetailsRepositoryMock.Verify(repo => repo.GetAllAsync(), Times.Once);
	}

	[Fact]
	public async Task GetAllBiographicalInfos_WhenNoDataExists_ShouldReturnEmptyCollectionOfBiographicalInfo()
	{
		//Arrange
		_biographicalDetailsRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync((ICollection<BiographicalData>?)null);

		//Act
		var result = await _biographicalDetailsService.GetAllBiographicalInfosAsync();

		//Assert
		Assert.NotNull(result);
		Assert.Empty(result);
		_biographicalDetailsRepositoryMock.Verify(repo => repo.GetAllAsync(), Times.Once);
	}

#endregion GETALL

	#region UPDATE

	[Fact]
	public async Task UpdateBiographicalInfo_ForNullUCIRequiredData_ShouldThrowException()
	{
		//Arrange
		int biographicalDataId = 1;

		var originalBiographicalData = new BiographicalData
		{
			Id = biographicalDataId,
			FirstName = "Ivan",
			LastName = "Perez",
			Email = "ivan@test.com",
			PreferredPronouns = "He/Him",
			LevelOfStudy = LevelOfStudy.SomeCollege,
			ImmigrationStatus = ImmigrationStatus.Visitor,
			SocialInsuranceNumber = null,
			UniqueClientIdentifier = null
		};

		var updatingBiographicalData = new BiographicalData
		{
			Id = biographicalDataId,
			FirstName = "Ivan",
			LastName = "Perez",
			Email = "perez@test2.com",
			PreferredPronouns = "He/Him",
			LevelOfStudy = LevelOfStudy.SomeCollege,
			ImmigrationStatus = ImmigrationStatus.Visitor,
			SocialInsuranceNumber = null,
			UniqueClientIdentifier = null
		};

		_biographicalDetailsRepositoryMock.Setup(repo => repo.GetAsync(biographicalDataId)).ReturnsAsync(originalBiographicalData);
		_biographicalDetailsRepositoryMock.Setup(repo => repo.UpdateAsync(updatingBiographicalData)).ReturnsAsync(false);

		//Act
		var result = async () => await _biographicalDetailsService.UpdateBiographicalInfoAsync(updatingBiographicalData);

		//Assert
		var exception = await Assert.ThrowsAsync<UCIException>(result);
		Assert.Equal(BiographicalDetailsErrors.RequiredUCI_Missing, exception.Message);
		_biographicalDetailsRepositoryMock.Verify(repo => repo.GetAsync(biographicalDataId), Times.Never);
		_biographicalDetailsRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<BiographicalData>()), Times.Never);
	}

	[Theory]
	[InlineData("")]
	[InlineData("0-0")]
	[InlineData("12345-123")]
	[InlineData("123-45678")]
	public async Task UpdateBiographicalInfo_ForInvalidUCIData_ShouldThrowException(string? uci)
	{
		//Arrange
		int biographicalDataId = 1;

		var originalBiographicalData = new BiographicalData
		{
			Id = biographicalDataId,
			FirstName = "Ivan",
			LastName = "Perez",
			Email = "ivan@test.com",
			PreferredPronouns = "He/Him",
			LevelOfStudy = LevelOfStudy.SomeCollege,
			ImmigrationStatus = ImmigrationStatus.Visitor,
			SocialInsuranceNumber = null,
			UniqueClientIdentifier = null
		};

		var updatingBiographicalData = new BiographicalData
		{
			Id = biographicalDataId,
			FirstName = "Ivan",
			LastName = "Perez",
			Email = "perez@test2.com",
			PreferredPronouns = "He/Him",
			LevelOfStudy = LevelOfStudy.SomeCollege,
			ImmigrationStatus = ImmigrationStatus.Visitor,
			SocialInsuranceNumber = null,
			UniqueClientIdentifier = uci
		};

		_biographicalDetailsRepositoryMock.Setup(repo => repo.GetAsync(biographicalDataId)).ReturnsAsync(originalBiographicalData);
		_biographicalDetailsRepositoryMock.Setup(repo => repo.UpdateAsync(updatingBiographicalData)).ReturnsAsync(false);

		//Act
		var result = async () => await _biographicalDetailsService.UpdateBiographicalInfoAsync(updatingBiographicalData);

		//Assert
		var exception = await Assert.ThrowsAsync<UCIException>(result);
		Assert.Equal(BiographicalDetailsErrors.UCIFormat_Invalid, exception.Message);
		_biographicalDetailsRepositoryMock.Verify(repo => repo.GetAsync(biographicalDataId), Times.Never);
		_biographicalDetailsRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<BiographicalData>()), Times.Never);
	}

	[Fact]
	public async Task UpdateBiographicalInfo_ForNullSINRequiredData_ShouldThrowException()
	{
		//Arrange
		int biographicalDataId = 1;

		var originalBiographicalData = new BiographicalData
		{
			Id = biographicalDataId,
			FirstName = "Ivan",
			LastName = "Perez",
			Email = "ivan@test.com",
			PreferredPronouns = "He/Him",
			LevelOfStudy = LevelOfStudy.SomeCollege,
			ImmigrationStatus = ImmigrationStatus.CanadianCitizen,
			SocialInsuranceNumber = null,
			UniqueClientIdentifier = null
		};

		var updatingBiographicalData = new BiographicalData
		{
			Id = biographicalDataId,
			FirstName = "Ivan",
			LastName = "Perez",
			Email = "perez@test2.com",
			PreferredPronouns = "He/Him",
			LevelOfStudy = LevelOfStudy.SomeCollege,
			ImmigrationStatus = ImmigrationStatus.CanadianCitizen,
			SocialInsuranceNumber = null,
			UniqueClientIdentifier = null
		};

		_biographicalDetailsRepositoryMock.Setup(repo => repo.GetAsync(biographicalDataId)).ReturnsAsync(originalBiographicalData);
		_biographicalDetailsRepositoryMock.Setup(repo => repo.UpdateAsync(updatingBiographicalData)).ReturnsAsync(false);

		//Act
		var result = async () => await _biographicalDetailsService.UpdateBiographicalInfoAsync(updatingBiographicalData);

		//Assert
		var exception = await Assert.ThrowsAsync<SINException>(result);
		Assert.Equal(BiographicalDetailsErrors.RequiredSIN_Missing, exception.Message);
		_biographicalDetailsRepositoryMock.Verify(repo => repo.GetAsync(biographicalDataId), Times.Never);
		_biographicalDetailsRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<BiographicalData>()), Times.Never);
	}

	[Theory]
	[InlineData("")]
	[InlineData("987-1654321")]
	[InlineData("1111-11-111")]
	[InlineData("004424156")]
	[InlineData("004 424 156")]
	public async Task UpdateBiographicalInfo_ForInvalidSINData_ShouldThrowException(string? sin)
	{
		//Arrange
		int biographicalDataId = 1;

		var originalBiographicalData = new BiographicalData
		{
			Id = biographicalDataId,
			FirstName = "Ivan",
			LastName = "Perez",
			Email = "ivan@test.com",
			PreferredPronouns = "He/Him",
			LevelOfStudy = LevelOfStudy.SomeCollege,
			ImmigrationStatus = ImmigrationStatus.CanadianCitizen,
			SocialInsuranceNumber = null,
			UniqueClientIdentifier = null
		};

		var updatingBiographicalData = new BiographicalData
		{
			Id = biographicalDataId,
			FirstName = "Ivan",
			LastName = "Perez",
			Email = "perez@test2.com",
			PreferredPronouns = "He/Him",
			LevelOfStudy = LevelOfStudy.SomeCollege,
			ImmigrationStatus = ImmigrationStatus.CanadianCitizen,
			SocialInsuranceNumber = sin,
			UniqueClientIdentifier = null
		};

		_biographicalDetailsRepositoryMock.Setup(repo => repo.GetAsync(biographicalDataId)).ReturnsAsync(originalBiographicalData);
		_biographicalDetailsRepositoryMock.Setup(repo => repo.UpdateAsync(updatingBiographicalData)).ReturnsAsync(false);

		//Act
		var result = async () => await _biographicalDetailsService.UpdateBiographicalInfoAsync(updatingBiographicalData);

		//Assert
		var exception = await Assert.ThrowsAsync<SINException>(result);
		Assert.Equal(BiographicalDetailsErrors.SINFormat_Invalid, exception.Message);
		_biographicalDetailsRepositoryMock.Verify(repo => repo.GetAsync(biographicalDataId), Times.Never);
		_biographicalDetailsRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<BiographicalData>()), Times.Never);
	}

	[Fact]
	public async Task UpdateBiographicalInfo_ForValidData_ShouldSaveUpdates()
	{
		//Arrange
		int biographicalDataId = 1;

		var originalBiographicalData = new BiographicalData
		{
			Id = biographicalDataId,
			FirstName = "Ivan",
			LastName = "Perez",
			Email = "ivan@test.com",
			PreferredPronouns = "He/Him",
			LevelOfStudy = LevelOfStudy.SomeCollege,
			ImmigrationStatus = ImmigrationStatus.Visitor,
			SocialInsuranceNumber = null,
			UniqueClientIdentifier = null
		};

		var updatingBiographicalData = new BiographicalData
		{
			Id = biographicalDataId,
			FirstName = "Ivan",
			LastName = "Perez",
			Email = "perez@test2.com",
			PreferredPronouns = "He/Him",
			LevelOfStudy = LevelOfStudy.SomeCollege,
			ImmigrationStatus = ImmigrationStatus.Visitor,
			SocialInsuranceNumber = null,
			UniqueClientIdentifier = "0000-0000"
		};

		_biographicalDetailsRepositoryMock.Setup(repo => repo.GetAsync(biographicalDataId)).ReturnsAsync(originalBiographicalData);
		_biographicalDetailsRepositoryMock.Setup(repo => repo.UpdateAsync(updatingBiographicalData)).ReturnsAsync(true);

		//Act
		var result = await _biographicalDetailsService.UpdateBiographicalInfoAsync(updatingBiographicalData);

		//Assert
		Assert.True(result);
		_biographicalDetailsRepositoryMock.Verify(repo => repo.GetAsync(biographicalDataId), Times.Once);
		_biographicalDetailsRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<BiographicalData>()), Times.Once);
	}

	[Fact]
	public async Task UpdateBiographicalInfo_WhenUpdatingUCI_ShouldThrowException()
	{
		//Arrange
		int biographicalDataId = 1;

		var originalBiographicalData = new BiographicalData
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

		var updatingBiographicalData = new BiographicalData
		{
			Id = biographicalDataId,
			FirstName = "Ivan",
			LastName = "Perez",
			Email = "perez@test2.com",
			PreferredPronouns = "He/Him",
			LevelOfStudy = LevelOfStudy.SomeCollege,
			ImmigrationStatus = ImmigrationStatus.Visitor,
			SocialInsuranceNumber = null,
			UniqueClientIdentifier = "0000-0001"
		};

		_biographicalDetailsRepositoryMock.Setup(repo => repo.GetAsync(biographicalDataId)).ReturnsAsync(originalBiographicalData);
		_biographicalDetailsRepositoryMock.Setup(repo => repo.UpdateAsync(updatingBiographicalData)).ReturnsAsync(false);

		//Act
		var result = async () => await _biographicalDetailsService.UpdateBiographicalInfoAsync(updatingBiographicalData);

		//Assert
		var exception = await Assert.ThrowsAsync<UCIException>(result);
		Assert.Equal(BiographicalDetailsErrors.AlreadySet_UCI_CannotUpdate, exception.Message);
		_biographicalDetailsRepositoryMock.Verify(repo => repo.GetAsync(biographicalDataId), Times.Once);
		_biographicalDetailsRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<BiographicalData>()), Times.Never);
	}

	[Fact]
	public async Task UpdateBiographicalInfo_WhenUpdatingSIN_ShouldThrowException()
	{
		//Arrange
		int biographicalDataId = 1;
		var originalBiographicalData = new BiographicalData
		{
			Id = biographicalDataId,
			FirstName = "Ivan",
			LastName = "Perez",
			Email = "ivan@test.com",
			PreferredPronouns = "He/Him",
			LevelOfStudy = LevelOfStudy.SomeCollege,
			ImmigrationStatus = ImmigrationStatus.CanadianCitizen,
			SocialInsuranceNumber = "000-000-000",
			UniqueClientIdentifier = null
		};

		var updatingBiographicalData = new BiographicalData
		{
			Id = biographicalDataId,
			FirstName = "Ivan",
			LastName = "Perez",
			Email = "perez@test2.com",
			PreferredPronouns = "He/Him",
			LevelOfStudy = LevelOfStudy.SomeCollege,
			ImmigrationStatus = ImmigrationStatus.CanadianCitizen,
			SocialInsuranceNumber = "004-424-156",
			UniqueClientIdentifier = null
		};

		_biographicalDetailsRepositoryMock.Setup(repo => repo.GetAsync(biographicalDataId)).ReturnsAsync(originalBiographicalData);
		_biographicalDetailsRepositoryMock.Setup(repo => repo.UpdateAsync(updatingBiographicalData)).ReturnsAsync(true);

		//Act
		var result = async () => await _biographicalDetailsService.UpdateBiographicalInfoAsync(updatingBiographicalData);

		//Assert
		var exception = await Assert.ThrowsAsync<SINException>(result);
		Assert.Equal(BiographicalDetailsErrors.AlreadySet_SIN_CannotUpdate, exception.Message);
		_biographicalDetailsRepositoryMock.Verify(repo => repo.GetAsync(biographicalDataId), Times.Once);
		_biographicalDetailsRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<BiographicalData>()), Times.Never);
	}

	#endregion UPDATE


	#region DELETE

	[Fact]
	public async Task DeleteBiographicalInfo_ForExistingBiographicalData_ShouldReturnBiographicalData()
	{
		//Arrange
		var biographicalDataId = 1;
		_biographicalDetailsRepositoryMock.Setup(repo => repo.DeleteAsync(biographicalDataId)).ReturnsAsync(true);

		//Act
		var result = await _biographicalDetailsService.DeleteBiographicalInfoAsync(biographicalDataId);

		//Assert
		Assert.True(result);
		_biographicalDetailsRepositoryMock.Verify(repo => repo.DeleteAsync(biographicalDataId), Times.Once);
	}

	[Fact]
	public async Task DeleteBiographicalInfo_ForNonExistentData_ShouldReturnFalse()
	{
		//Arrange
		var biographicalDataId = 9999;
		_biographicalDetailsRepositoryMock.Setup(repo => repo.DeleteAsync(biographicalDataId)).ReturnsAsync(false);

		//Act
		var result = await _biographicalDetailsService.DeleteBiographicalInfoAsync(biographicalDataId);

		//Assert
		Assert.False(result);
		_biographicalDetailsRepositoryMock.Verify(repo => repo.DeleteAsync(biographicalDataId), Times.Once);
	}

	#endregion DELETE
}
