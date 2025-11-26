using BiographicalDetails.Application.Errors;
using BiographicalDetails.Domain;
using BiographicalDetails.Domain.Abstractions;

namespace BiographicalDetails.Application.Validators;

public class BiographicalDataValidator : IBiographicalDataValidator
{
	IStringValidator _sinValidator;
	IStringValidator _uciValidator;
	public BiographicalDataValidator(IValidatorSIN sinValidator, IValidatorUCI uciValidator)
	{
		_sinValidator = sinValidator;
		_uciValidator = uciValidator;
	}

	public void ValidateData(BiographicalData data)
	{
		if (ImmigrationStatusRequiresSIN(data.ImmigrationStatus))
		{
			if (data.SocialInsuranceNumber is null)
				throw new NullReferenceException(BiographicalDetailsErrors.RequiredSIN_Missing);

			if (!_sinValidator.IsValid(data.SocialInsuranceNumber, out string errorMsg))
				throw new InvalidDataException(errorMsg);
		}

		if (ImmigrationStatusRequiresUCI(data.ImmigrationStatus))
		{
			if (data.UniqueClientIdentifier is null)
				throw new NullReferenceException(BiographicalDetailsErrors.RequiredUCI_Missing);

			if (!_uciValidator.IsValid(data.UniqueClientIdentifier, out string errorMsg))
				throw new InvalidDataException(errorMsg);
		}
	}

	private bool ImmigrationStatusRequiresSIN(ImmigrationStatus immigrationStatus)
	{
		return immigrationStatus is (ImmigrationStatus.CanadianCitizen
			or ImmigrationStatus.PermanentResident
			or ImmigrationStatus.TemporaryForeignWorker
			or ImmigrationStatus.ProtectedPerson
			or ImmigrationStatus.Indigenous);
	}

	private bool ImmigrationStatusRequiresUCI(ImmigrationStatus immigrationStatus)
	{
		return immigrationStatus is (ImmigrationStatus.PermanentResident
			or ImmigrationStatus.TemporaryForeignWorker
			or ImmigrationStatus.InternationalStudent
			or ImmigrationStatus.ProtectedPerson
			or ImmigrationStatus.Visitor);
	}
}
