using BiographicalDetails.Application.Errors;
using BiographicalDetails.Application.Validators;

namespace BiographicalDetails.Unit.Tests;

public class SINValidatorTests
{
	SINValidator _sinValidator;
	public SINValidatorTests()
	{
		_sinValidator = new SINValidator();
	}

	[Fact]
	public void IsValid_WhenNotASCIINumbers_ShouldReturnFalseAndErrorMsg()
	{
		//Arrange
		var invalidFormatSIN = "௯௯௯-௯௯௯-௯௯௯"; //999-999-999 in arabic

		//Act
		var result = _sinValidator.IsValid(invalidFormatSIN, out string errorMsg);

		//Assert
		Assert.False(result);
		Assert.Equal(BiographicalDetailsErrors.SIN_Invalid, errorMsg);
	}

	[Theory]
	[InlineData("000-000-000")] // 0 is divisible by 10
	[InlineData("123-466-989")] // 50 ...
	[InlineData("004-424-156")] // 30 ...
	public void IsValid_ForValidData_ShouldReturnTrueAndEmptyErrorMsg(string sin)
	{
		//Arrange
		var validSIN = sin;

		//Act
		var result = _sinValidator.IsValid(validSIN, out string errorMsg);

		//Assert
		Assert.True(result);
		Assert.Equal(String.Empty, errorMsg);
	}

	[Theory]
	[InlineData("123-456-789")] // 47 not divisible by 10
	[InlineData("123-466-889")] // 49 ...
	[InlineData("046-454-386")] // 51
	public void IsValid_ForInvalidData_ShouldReturnFalseAndErrorMsg(string sin)
	{
		//Arrange
		var validSIN = sin;

		//Act
		var result = _sinValidator.IsValid(validSIN, out string errorMsg);

		//Assert
		Assert.False(result);
		Assert.Equal(BiographicalDetailsErrors.SIN_Invalid, errorMsg);
	}

	[Theory]
	[InlineData("")]
	[InlineData("000-00-000")]
	[InlineData("00-0000-000")]
	[InlineData("1234-669-89")]
	[InlineData("000 000 000")]
	[InlineData("000000000")]
	public void IsValid_ForInvalidFormat_ShouldReturnFalseAndErrorMsg(string sin)
	{
		//Arrange
		var validSIN = sin;

		//Act
		var result = _sinValidator.IsValid(validSIN, out string errorMsg);

		//Assert
		Assert.False(result);
		Assert.Equal(BiographicalDetailsErrors.SINFormat_Invalid, errorMsg);
	}
}
