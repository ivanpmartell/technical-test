using BiographicalDetails.Application.Errors;
using BiographicalDetails.Domain;
using BiographicalDetails.EntityModels.Abstractions;

namespace BiographicalDetails.EntityModels.Mappers;

public class BiographicalDataEntityMapper : IBiographicalDataMapper
{
	public UserEntity MapToUser(BiographicalData domainModel)
	{
		var user = new UserEntity
		{
			Id = domainModel.Id,
			FirstName = domainModel.FirstName,
			LastName = domainModel.LastName,
			Email = domainModel.Email,
			LevelOfStudy = domainModel.LevelOfStudy,
			ImmigrationStatus = domainModel.ImmigrationStatus
		};
		return user;
	}

	public UserEntity MapToUserWithoutId(BiographicalData domainModel)
	{
		var user = new UserEntity
		{
			Id = 0,
			FirstName = domainModel.FirstName,
			LastName = domainModel.LastName,
			Email = domainModel.Email,
			LevelOfStudy = domainModel.LevelOfStudy,
			ImmigrationStatus = domainModel.ImmigrationStatus
		};
		return user;
	}

	public UserPronounEntity MapToUserPronouns(BiographicalData domainModel)
	{
		if (domainModel.PreferredPronouns is null)
			throw new NullReferenceException(BiographicalDataMapperErrors.UserPronouns_Missing);

		var userPronoun = new UserPronounEntity
		{
			Id = 0,
			UserId = domainModel.Id,
			PreferredPronouns = domainModel.PreferredPronouns
		};
		return userPronoun;
	}

	public UserSinEntity MapToUserSIN(BiographicalData domainModel)
	{
		if (domainModel.SocialInsuranceNumber is null)
			throw new NullReferenceException(BiographicalDataMapperErrors.UserSin_Missing);

		var userSin = new UserSinEntity
		{
			Id = 0,
			UserId = domainModel.Id,
			SocialInsuranceNumber = domainModel.SocialInsuranceNumber
		};
		return userSin;
	}

	public UserUciEntity MapToUserUCI(BiographicalData domainModel)
	{
		if (domainModel.UniqueClientIdentifier is null)
			throw new NullReferenceException(BiographicalDataMapperErrors.UserUci_Missing);

		var userUci = new UserUciEntity
		{
			Id = 0,
			UserId = domainModel.Id,
			UniqueClientIdentifier = domainModel.UniqueClientIdentifier
		};
		return userUci;
	}

	public BiographicalData MapFrom(UserEntity user, UserPronounEntity? userPronouns = null, UserSinEntity? userSin = null, UserUciEntity? userUci = null)
	{
		string? pronouns = null;
		if (userPronouns is not null)
			pronouns = userPronouns.PreferredPronouns;

		string? sin = null;
		if (userSin is not null)
			sin = userSin.SocialInsuranceNumber;

		string? uci = null;
		if (userUci is not null)
			uci = userUci.UniqueClientIdentifier;

		return new BiographicalData
		{
			Id = user.Id,
			FirstName = user.FirstName,
			LastName = user.LastName,
			Email = user.Email,
			PreferredPronouns = pronouns,
			LevelOfStudy = user.LevelOfStudy,
			ImmigrationStatus = user.ImmigrationStatus,
			SocialInsuranceNumber = sin,
			UniqueClientIdentifier = uci
		};
	}

	public void MapToUserByRef(BiographicalData domainModel, UserEntity user)
	{
		if (user is null)
			throw new ArgumentNullException(BiographicalDataMapperErrors.CannotUpdateUserReferenceOfNull);

		user.FirstName = domainModel.FirstName;
		user.LastName = domainModel.LastName;
		user.Email = domainModel.Email;
		user.LevelOfStudy = domainModel.LevelOfStudy;
		user.ImmigrationStatus = domainModel.ImmigrationStatus;
	}

	public void MapToUserPronounsByRef(BiographicalData domainModel, UserPronounEntity userPronoun)
	{
		if (userPronoun is null)
			throw new ArgumentNullException(BiographicalDataMapperErrors.CannotUpdateUserPronounsReferenceOfNull);

		if (domainModel.PreferredPronouns is null)
			throw new NullReferenceException(BiographicalDataMapperErrors.UserPronouns_Missing);

		userPronoun.UserId = domainModel.Id;
		userPronoun.PreferredPronouns = domainModel.PreferredPronouns;
	}

	public void MapToUserSINByRef(BiographicalData domainModel, UserSinEntity userSin)
	{
		if (userSin is null)
			throw new ArgumentNullException(BiographicalDataMapperErrors.CannotUpdateUserSinReferenceOfNull);

		if (domainModel.SocialInsuranceNumber is null)
			throw new NullReferenceException(BiographicalDataMapperErrors.UserSin_Missing);

		userSin.UserId = domainModel.Id;
		userSin.SocialInsuranceNumber = domainModel.SocialInsuranceNumber;
	}

	public void MapToUserUCIByRef(BiographicalData domainModel, UserUciEntity userUci)
	{
		if (userUci is null)
			throw new ArgumentNullException(BiographicalDataMapperErrors.CannotUpdateUserUciReferenceOfNull);

		if (domainModel.UniqueClientIdentifier is null)
			throw new NullReferenceException(BiographicalDataMapperErrors.UserUci_Missing);

		userUci.UserId = domainModel.Id;
		userUci.UniqueClientIdentifier = domainModel.UniqueClientIdentifier;
	}

	public BiographicalDataEntity MapToEntity(BiographicalData domainModel)
	{
		var entity = new BiographicalDataEntity
		{
			Id = domainModel.Id,
			FirstName = domainModel.FirstName,
			LastName = domainModel.LastName,
			Email = domainModel.Email,
			PreferredPronouns = domainModel.PreferredPronouns,
			LevelOfStudy = domainModel.LevelOfStudy,
			ImmigrationStatus = domainModel.ImmigrationStatus,
			SocialInsuranceNumber = domainModel.SocialInsuranceNumber,
			UniqueClientIdentifier = domainModel.UniqueClientIdentifier
		};
		return entity;
	}

	public BiographicalDataEntity MapToEntityWithoutId(BiographicalData domainModel)
	{
		var entity = new BiographicalDataEntity
		{
			Id = 0,
			FirstName = domainModel.FirstName,
			LastName = domainModel.LastName,
			Email = domainModel.Email,
			PreferredPronouns = domainModel.PreferredPronouns,
			LevelOfStudy = domainModel.LevelOfStudy,
			ImmigrationStatus = domainModel.ImmigrationStatus,
			SocialInsuranceNumber = domainModel.SocialInsuranceNumber,
			UniqueClientIdentifier = domainModel.UniqueClientIdentifier
		};
		return entity;
	}

	public void MapToEntityByRef(BiographicalData domainModel, BiographicalDataEntity entity)
	{
		entity.Id = domainModel.Id;
		entity.FirstName = domainModel.FirstName;
		entity.LastName = domainModel.LastName;
		entity.Email = domainModel.Email;
		entity.PreferredPronouns = domainModel.PreferredPronouns;
		entity.LevelOfStudy = domainModel.LevelOfStudy;
		entity.ImmigrationStatus = domainModel.ImmigrationStatus;
		entity.SocialInsuranceNumber = domainModel.SocialInsuranceNumber;
		entity.UniqueClientIdentifier = domainModel.UniqueClientIdentifier;
	}
}
