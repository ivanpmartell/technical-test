using System.Data;
using BiographicalDetails.Domain;
using BiographicalDetails.Domain.Abstractions;
using BiographicalDetails.EntityModels;
using BiographicalDetails.EntityModels.Abstractions;
using BiographicalDetails.EntityModels.Mappers;
using BiographicalDetails.Infrastructure.InMemory.Contexts;
using BiographicalDetails.Infrastructure.InMemory.Errors;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace BiographicalDetails.Infrastructure.InMemory;

public class InMemoryBiographicalDataRepository : IBiographicalDataRepository
{
	private readonly BiographicalDataDbContext _context;
	private readonly IBiographicalDataMapper _mapper;

	public InMemoryBiographicalDataRepository(BiographicalDataDbContext context, IBiographicalDataMapper mapper)
	{
		_context = context;
		_mapper = mapper;
	}
	public async Task<BiographicalData?> AddAsync(BiographicalData biographicalData)
	{
		if (biographicalData is null)
			throw new NullReferenceException(InMemoryBiographicalDataRepositoryErrors.BiographicalData_IsNull);

		var user = _mapper.MapToUser(biographicalData);
		var addedUser = await _context.AddAsync(user);

		EntityEntry<UserPronounEntity>? addedPronouns = null;
		if (biographicalData.PreferredPronouns is not null)
		{
			var pronouns = _mapper.MapToUserPronouns(biographicalData);
			addedPronouns = await _context.AddAsync(pronouns);
		}

		EntityEntry<UserSinEntity>? addedSin = null;
		if (biographicalData.SocialInsuranceNumber is not null)
		{
			var sin = _mapper.MapToUserSIN(biographicalData);
			addedSin = await _context.AddAsync(sin);
		}

		EntityEntry<UserUciEntity>? addedUci = null;
		if (biographicalData.UniqueClientIdentifier is not null)
		{
			var uci = _mapper.MapToUserUCI(biographicalData);
			addedUci = await _context.AddAsync(uci);
		}
		
		var rowsAffected = await _context.SaveChangesAsync();

		if (rowsAffected == 0)
			return null;

		return _mapper.MapFrom(addedUser.Entity, addedPronouns?.Entity, addedSin?.Entity, addedUci?.Entity);
	}

	public async Task<bool> DeleteAsync(int biographicalDataId)
	{
		int affected = 0;
		var dataInDb = await _context.Users.FindAsync(biographicalDataId);
		if (dataInDb is not null)
		{
			_context.Users.Remove(dataInDb);
			affected = await _context.SaveChangesAsync();
		}

		if (affected == 0)
			return false;
		return true;
	}

	public async Task<ICollection<BiographicalData>?> GetAllAsync()
	{
		var result =
			from users in _context.Users
			from pronouns in _context.UserPronouns.Where(pronoun => pronoun.UserId == users.Id).DefaultIfEmpty()
			from sins in _context.UserSins.Where(sin => sin.UserId == users.Id).DefaultIfEmpty()
			from ucis in _context.UserUcis.Where(uci => uci.UserId == users.Id).DefaultIfEmpty()
			select new BiographicalData
			{
				Id = users.Id,
				FirstName = users.FirstName,
				LastName = users.LastName,
				Email = users.Email,
				PreferredPronouns = pronouns == null ? null : pronouns.PreferredPronouns,
				LevelOfStudy = users.LevelOfStudy,
				ImmigrationStatus = users.ImmigrationStatus,
				SocialInsuranceNumber = sins == null ? null : sins.SocialInsuranceNumber,
				UniqueClientIdentifier = ucis == null ? null : ucis.UniqueClientIdentifier
			};
		return await result.ToListAsync();
	}

	public async Task<BiographicalData?> GetAsync(int biographicalDataId)
	{
		var result =
			from users in _context.Users where users.Id == biographicalDataId
			from pronouns in _context.UserPronouns.Where(pronoun => pronoun.UserId == users.Id).DefaultIfEmpty()
			from sins in _context.UserSins.Where(sin => sin.UserId == users.Id).DefaultIfEmpty()
			from ucis in _context.UserUcis.Where(uci => uci.UserId == users.Id).DefaultIfEmpty()
			select new BiographicalData
			{
				Id = users.Id,
				FirstName = users.FirstName,
				LastName = users.LastName,
				Email = users.Email,
				PreferredPronouns = pronouns == null ? null : pronouns.PreferredPronouns,
				LevelOfStudy = users.LevelOfStudy,
				ImmigrationStatus = users.ImmigrationStatus,
				SocialInsuranceNumber = sins == null ? null : sins.SocialInsuranceNumber,
				UniqueClientIdentifier = ucis == null ? null : ucis.UniqueClientIdentifier
			};
		return await result.FirstOrDefaultAsync();
	}

	public async Task<bool> UpdateAsync(BiographicalData biographicalData)
	{
		if (biographicalData is null)
			throw new NullReferenceException(InMemoryBiographicalDataRepositoryErrors.BiographicalData_IsNull);

		var userInDb = await _context.Users.FindAsync(biographicalData.Id);
		if (userInDb is null)
			return false;

		_mapper.MapToUserByRef(biographicalData, userInDb);
		_context.Update(userInDb);

		if (biographicalData.PreferredPronouns is not null)
		{
			var userPronounsInDb = await _context.UserPronouns.FirstOrDefaultAsync(up => up.UserId == biographicalData.Id);

			if (userPronounsInDb is not null && userPronounsInDb.PreferredPronouns != biographicalData.PreferredPronouns)
			{
				_mapper.MapToUserPronounsByRef(biographicalData, userPronounsInDb);
				_context.Update(userPronounsInDb);
			}
			else if (userPronounsInDb is null)
			{
				var pronouns = _mapper.MapToUserPronouns(biographicalData);
				_context.Update(pronouns);
			}
		}

		if (biographicalData.SocialInsuranceNumber is not null)
		{
			var userSinInDb = await _context.UserSins.FirstOrDefaultAsync(us => us.UserId == biographicalData.Id);
			if (userSinInDb is not null && userSinInDb.SocialInsuranceNumber != biographicalData.SocialInsuranceNumber)
			{
				_mapper.MapToUserSINByRef(biographicalData, userSinInDb);
				_context.Update(userSinInDb);
			}
			else if (userSinInDb is null)
			{
				var sin = _mapper.MapToUserSIN(biographicalData);
				_context.Update(sin);
			}
		}

		if (biographicalData.UniqueClientIdentifier is not null)
		{
			var userUciInDb = await _context.UserUcis.FirstOrDefaultAsync(us => us.UserId == biographicalData.Id);
			if (userUciInDb is not null && userUciInDb.UniqueClientIdentifier != biographicalData.UniqueClientIdentifier)
			{
				_mapper.MapToUserUCIByRef(biographicalData, userUciInDb);
				_context.Update(userUciInDb);
			}
			else if (userUciInDb is null)
			{
				var uci = _mapper.MapToUserUCI(biographicalData);
				_context.Update(uci);
			}
		}

		var rowsAffected = await _context.SaveChangesAsync();

		if (rowsAffected == 0)
			return false;
		return true;
	}
}
