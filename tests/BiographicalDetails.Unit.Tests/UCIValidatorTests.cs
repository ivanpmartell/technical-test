using BiographicalDetails.Application.Errors;
using BiographicalDetails.Application.Validators;

namespace BiographicalDetails.Unit.Tests;

public class UCIValidatorTests
{
	UCIValidator _uciValidator;
	public UCIValidatorTests()
	{
		_uciValidator = new UCIValidator();
	}

	[Theory]
	[InlineData("0000-0000")]
	[InlineData("1234-5678")]
	[InlineData("9999-9999")]
	[InlineData("9876-5432")]
	public void IsValid_ForValidFormat_ShouldReturnTrueAndEmptyErrorMsg(string uci)
	{
		//Arrange
		var validUCI = uci;

		//Act
		var result = _uciValidator.IsValid(validUCI, out string errorMsg);

		//Assert
		Assert.True(result);
		Assert.Equal(String.Empty, errorMsg);
	}

	[Theory]
	[InlineData("")]
	[InlineData("000-00-000")]
	[InlineData("00-0000-00")]
	[InlineData("12345678")]
	[InlineData("1234 5678")]
	[InlineData("௯௯௯௯-௯௯௯௯")]
	public void IsValid_ForInvalidFormat_ShouldReturnFalseAndErrorMsg(string uci)
	{
		//Arrange
		var validUCI = uci;

		//Act
		var result = _uciValidator.IsValid(validUCI, out string errorMsg);

		//Assert
		Assert.False(result);
		Assert.Equal(BiographicalDetailsErrors.UCIFormat_Invalid, errorMsg);
	}
}
