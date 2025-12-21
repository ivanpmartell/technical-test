using System.Text.RegularExpressions;
using BiographicalDetails.Application.Errors;
using BiographicalDetails.Application.Utils;
using BiographicalDetails.Domain.Abstractions;

namespace BiographicalDetails.Application.Validators;

public class UCIValidator : IStringValidator
{
	public bool IsValid(string uci, out string errorMessage)
	{
		var validDigitsCount = 0; 
		for (int i = 0; i < uci.Length; i++)
		{
			var currentDigit = GlobalizationUtility.ValidCharToInt(uci[i]);

			if (currentDigit < 0)
				continue;

			validDigitsCount++;
		}

		var pattern = @"^\d{4}-\d{4}$|^\d{2}-\d{4}-\d{4}$";
		var match = Regex.Match(uci, pattern);

		if (!match.Success || (validDigitsCount != 8 && validDigitsCount != 10))
		{
			errorMessage = BiographicalDetailsErrors.UCIFormat_Invalid;
			return false;
		}

		errorMessage = String.Empty;
		return true;
	}
}
