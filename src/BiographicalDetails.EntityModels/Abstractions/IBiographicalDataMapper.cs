using BiographicalDetails.Domain;

namespace BiographicalDetails.EntityModels.Abstractions;

public interface IBiographicalDataMapper
{
	public UserEntity MapToUser(BiographicalData domainModel);
	public UserEntity MapToUserWithoutId(BiographicalData domainModel);
	public UserPronounEntity MapToUserPronouns(BiographicalData domainModel);
	public UserSinEntity MapToUserSIN(BiographicalData domainModel);
	public UserUciEntity MapToUserUCI(BiographicalData domainModel);


	public void MapToUserByRef(BiographicalData domainModel, UserEntity user);
	public void MapToUserPronounsByRef(BiographicalData domainModel, UserPronounEntity userPronoun);
	public void MapToUserSINByRef(BiographicalData domainModel, UserSinEntity userSin);
	public void MapToUserUCIByRef(BiographicalData domainModel, UserUciEntity userUci);


	public BiographicalDataEntity MapToEntity(BiographicalData domainModel);
	public BiographicalDataEntity MapToEntityWithoutId(BiographicalData domainModel);
	public void MapToEntityByRef(BiographicalData domainModel, BiographicalDataEntity entity);


	public BiographicalData MapFrom(UserEntity user, UserPronounEntity? userPronouns = null, UserSinEntity? userSin = null, UserUciEntity? userUci = null);

}
