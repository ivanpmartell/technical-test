using BiographicalDetails.Application.Utils;
using BiographicalDetails.Domain;
using BiographicalDetails.Website.Models.ViewModels;

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
			PreferredPronouns = StringUtility.EnsureNullIfEmpty(biographicalDetailsModel.PreferredPronouns),
			LevelOfStudy = (Domain.LevelOfStudy)biographicalDetailsModel.LevelOfStudy,
			ImmigrationStatus = (Domain.ImmigrationStatus)biographicalDetailsModel.ImmigrationStatus,
			SocialInsuranceNumber = StringUtility.EnsureNullIfEmpty(biographicalDetailsModel.SocialInsuranceNumber),
			UniqueClientIdentifier = StringUtility.EnsureNullIfEmpty(biographicalDetailsModel.UniqueClientIdentifier)
		};
		return biographicalData;
	}

	public SubmissionViewModel MapToSubmissionViewModel(BiographicalData domainModel)
	{
		var submissionViewModel = new SubmissionViewModel
		{
			Id = domainModel.Id,
			FirstName = domainModel.FirstName,
			LastName = domainModel.LastName,
			Email = domainModel.Email,
			LevelOfStudy = (Enums.LevelOfStudy)domainModel.LevelOfStudy,
			ImmigrationStatus = (Enums.ImmigrationStatus)domainModel.ImmigrationStatus,
			SocialInsuranceNumber = domainModel.SocialInsuranceNumber,
			UniqueClientIdentifier = domainModel.UniqueClientIdentifier
		};
		return submissionViewModel;
	}

	public IEnumerable<SubmissionViewModel> MapToBiographicalDetailsSubmissionList(ICollection<BiographicalData> domainModelCollection)
	{
		var submissionList = new List<SubmissionViewModel>();
		foreach (var item in domainModelCollection)
		{
			submissionList.Add(MapToSubmissionViewModel(item));
		}
		return submissionList;
	}
}
