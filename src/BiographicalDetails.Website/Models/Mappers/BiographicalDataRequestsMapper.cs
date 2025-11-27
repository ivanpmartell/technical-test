using BiographicalDetails.Domain;
using BiographicalDetails.Website.Models.Enums;

namespace BiographicalDetails.Website.Models.Mappers;

public class BiographicalDataRequestsMapper
{
	public BiographicalDetailsModel MapToBiographicalDetailsViewModel(BiographicalData domainModel)
	{
		var biographicalDetailsViewModel = new BiographicalDetailsModel
		{
			Id = domainModel.Id,
			FirstName = domainModel.FirstName,
			LastName = domainModel.LastName,
			Email = domainModel.Email,
			PreferredPronouns = domainModel.PreferredPronouns,
			LevelOfStudy = (Enums.LevelOfStudy)domainModel.LevelOfStudy,
			ImmigrationStatus = (Enums.ImmigrationStatus)domainModel.ImmigrationStatus,
			SocialInsuranceNumber = domainModel.SocialInsuranceNumber,
			UniqueClientIdentifier = domainModel.UniqueClientIdentifier
		};
		return biographicalDetailsViewModel;
	}

	public BiographicalData MapFrom(BiographicalDetailsModel biographicalDetailsModel)
	{
		var biographicalData = new BiographicalData
		{
			Id = biographicalDetailsModel.Id,
			FirstName = biographicalDetailsModel.FirstName,
			LastName = biographicalDetailsModel.LastName,
			Email = biographicalDetailsModel.Email,
			PreferredPronouns = biographicalDetailsModel.PreferredPronouns,
			LevelOfStudy = (Domain.LevelOfStudy)biographicalDetailsModel.LevelOfStudy,
			ImmigrationStatus = (Domain.ImmigrationStatus)biographicalDetailsModel.ImmigrationStatus,
			SocialInsuranceNumber = EnsureNullIfEmpty(biographicalDetailsModel.SocialInsuranceNumber),
			UniqueClientIdentifier = EnsureNullIfEmpty(biographicalDetailsModel.UniqueClientIdentifier)
		};
		return biographicalData;
	}

	private string? EnsureNullIfEmpty(string? value)
	{
		return String.IsNullOrEmpty(value) ?
			null : value;
	}

	public ICollection<BiographicalDetailsModel> MapToBiographicalDetailsViewModelCollection(ICollection<BiographicalData> domainModelCollection)
	{
		var biographicalDetailsViewModelCollection = new List<BiographicalDetailsModel>();
		foreach (var item in domainModelCollection)
		{
			biographicalDetailsViewModelCollection.Add(MapToBiographicalDetailsViewModel(item));
		}
		return biographicalDetailsViewModelCollection;
	}
}
