using System.ComponentModel.DataAnnotations;
using BiographicalDetails.Domain.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace BiographicalDetails.Website.Models.Validation;

public class SinAttribute : ValidationAttribute
{
	public const string FormatErrorMsg = "Social insurance number must have the following format: 000-000-000";
	public const string InvalidErrorMsg = "Social insurance number is invalid.";

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