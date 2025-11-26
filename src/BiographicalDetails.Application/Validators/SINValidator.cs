using System.Text.RegularExpressions;
using BiographicalDetails.Application.Errors;
using BiographicalDetails.Application.Utils;
using BiographicalDetails.Domain.Abstractions;

namespace BiographicalDetails.Application.Validators;

public class SINValidator : IValidatorSIN
{
	public bool IsValid(string sin, out string errorMessage)
	{
		if (!IsValidSINFormat(sin))
		{
			errorMessage = BiographicalDetailsErrors.SINFormat_Invalid;
			return false;
		}

		if (!LuhnsAlgorithm_IsValid(sin))
		{
			errorMessage = BiographicalDetailsErrors.SIN_Invalid;
			return false;
		}

		errorMessage = String.Empty;
		return true;
	}

	private bool IsValidSINFormat(string sin)
	{
		var pattern = @"^\d{3}-\d{3}-\d{3}$";
		var match = Regex.Match(sin, pattern);
		return match.Success;
	}

	private bool LuhnsAlgorithm_IsValid(string sin)
	{
		var sum = 0;
		var isEveryOtherDigit = false;
		bool hasAtLeastOneValidChar = false;

		for (int i = sin.Length - 1; i >= 0; i--)
		{
			var currentChar = sin[i];
			var currentDigit = GlobalizationUtility.ValidCharToInt(currentChar);

			if (currentDigit < 0)
				continue;

			if (!hasAtLeastOneValidChar)
				hasAtLeastOneValidChar = true;

			if (isEveryOtherDigit)
			{
				var multiplyBy2Result = currentDigit * 2;
				while (multiplyBy2Result != 0)
				{
					int multiplyBy2ResultDigit = multiplyBy2Result % 10;
					sum += multiplyBy2ResultDigit;
					multiplyBy2Result = multiplyBy2Result / 10;
				}
			}
			else
				sum += currentDigit;

			isEveryOtherDigit = !isEveryOtherDigit;
		}

		if (!hasAtLeastOneValidChar)
			return false;

		return sum % 10 == 0;
	}
}
