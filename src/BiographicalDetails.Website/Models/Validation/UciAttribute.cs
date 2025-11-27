using System.ComponentModel.DataAnnotations;
using BiographicalDetails.Domain.Abstractions;

namespace BiographicalDetails.Website.Models.Validation;

public class UciAttribute : ValidationAttribute
{
	protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
	{
		var uci = ((BiographicalDetailsModel)validationContext.ObjectInstance).UniqueClientIdentifier;

		//Requirement is checked elsewhere
		if (uci is null)
			return ValidationResult.Success;

		var uciValidator = validationContext.GetService<IValidatorUCI>();
		if (uciValidator is null)
			return new ValidationResult("Unique client identifier could not be validated.");

		

		if (!uciValidator.IsValid(uci, out string errorMsg))
			return new ValidationResult(errorMsg);

		return ValidationResult.Success;
	}
}
