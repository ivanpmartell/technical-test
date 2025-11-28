using System.ComponentModel.DataAnnotations;
using BiographicalDetails.Domain.Abstractions;

namespace BiographicalDetails.Website.Models.Validation;

public class SinAttribute : ValidationAttribute
{
	protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
	{
		var sin = ((BiographicalDetailsModel)validationContext.ObjectInstance).SocialInsuranceNumber;

		//Requirement is checked elsewhere
		if (sin is null)
			return ValidationResult.Success;

		var sinValidator = validationContext.GetService<IValidatorSIN>();
		if (sinValidator is null)
			return new ValidationResult("Social insurance number could not be validated.");

		if (!sinValidator.IsValid(sin, out string errorMsg))
			return new ValidationResult(errorMsg);

		return ValidationResult.Success;
	}
}