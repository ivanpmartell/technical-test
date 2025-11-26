using BiographicalDetails.Application.Errors;
using BiographicalDetails.Domain;
using BiographicalDetails.Domain.Abstractions;

namespace BiographicalDetails.Application.Services;

public class BiographicalDetailsService
{
	private readonly IBiographicalDataRepository _biographicalDetailsRepository;
	private readonly IBiographicalDataValidator _biographicalDetailsValidator;

	public BiographicalDetailsService(IBiographicalDataRepository biographicalDetailsRepository, IBiographicalDataValidator biographicalDetailsValidator)
	{
		_biographicalDetailsRepository = biographicalDetailsRepository;
		_biographicalDetailsValidator = biographicalDetailsValidator;
	}

	public async Task<BiographicalData> SaveBiographicalInfoAsync(BiographicalData biographicalData)
	{
		_biographicalDetailsValidator.ValidateData(biographicalData);

		await _biographicalDetailsRepository.AddAsync(biographicalData);

		return biographicalData;
	}

	public async Task<BiographicalData> GetBiographicalInfoAsync(int biographicalDataId)
	{
		var result = await _biographicalDetailsRepository.GetAsync(biographicalDataId);

		if (result is null)
			throw new KeyNotFoundException(BiographicalDetailsErrors.BiographicalDetails_NotFound);

		return result;
	}

	public async Task<ICollection<BiographicalData>> GetAllBiographicalInfosAsync()
	{
		var result = await _biographicalDetailsRepository.GetAllAsync();

		if (result is null)
			return [];

		return result;
	}

	public async Task<bool> UpdateBiographicalInfoAsync(BiographicalData updatedBiographicalData)
	{
		_biographicalDetailsValidator.ValidateData(updatedBiographicalData);

		var currentBiographicalData = await GetBiographicalInfoAsync(updatedBiographicalData.Id);

		if (currentBiographicalData.SocialInsuranceNumber is not null &&
				currentBiographicalData.SocialInsuranceNumber != updatedBiographicalData.SocialInsuranceNumber)
			throw new InvalidOperationException(BiographicalDetailsErrors.AlreadySet_SIN_CannotUpdate);

		if (currentBiographicalData.UniqueClientIdentifier is not null &&
			currentBiographicalData.UniqueClientIdentifier != updatedBiographicalData.UniqueClientIdentifier)
			throw new InvalidOperationException(BiographicalDetailsErrors.AlreadySet_UCI_CannotUpdate);

		return await _biographicalDetailsRepository.UpdateAsync(updatedBiographicalData);
	}

	public async Task<bool> DeleteBiographicalInfoAsync(int biographicalDataId)
	{
		var result = await _biographicalDetailsRepository.DeleteAsync(biographicalDataId);

		if (!result)
			throw new KeyNotFoundException(BiographicalDetailsErrors.BiographicalDetails_NotFound);

		return result;
	}
}