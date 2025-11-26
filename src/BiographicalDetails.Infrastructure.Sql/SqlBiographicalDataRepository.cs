using BiographicalDetails.Domain;
using BiographicalDetails.Domain.Abstractions;
using BiographicalDetails.EntityModels.Abstractions;
using BiographicalDetails.Infrastructure.InMemory.Errors;
using BiographicalDetails.Infrastructure.Sql.Contexts;
using Microsoft.EntityFrameworkCore;

namespace BiographicalDetails.Infrastructure.Sql;

public class SqlBiographicalDataRepository : IBiographicalDataRepository
{
	private readonly BiographicalDataDbContext _context;
	private readonly IBiographicalDataMapper _mapper;

	public SqlBiographicalDataRepository(BiographicalDataDbContext context, IBiographicalDataMapper mapper)
	{
		_context = context;
		_mapper = mapper;
	}

	public async Task<BiographicalData?> AddAsync(BiographicalData biographicalData)
	{
		if (biographicalData is null)
			throw new NullReferenceException(SqlBiographicalDataRepositoryErrors.BiographicalData_IsNull);

		var entity = _mapper.MapToEntityWithoutId(biographicalData);
		var addedEntity = await _context.AddAsync(entity);
		var affectedRows = await _context.SaveChangesAsync();
		if (affectedRows == 0 || addedEntity is null)
			return null;


		return new BiographicalData
		{
			Id = addedEntity.Entity.Id,
			FirstName = addedEntity.Entity.FirstName,
			LastName = addedEntity.Entity.LastName,
			Email = addedEntity.Entity.Email,
			PreferredPronouns = addedEntity.Entity.PreferredPronouns,
			LevelOfStudy = addedEntity.Entity.LevelOfStudy,
			ImmigrationStatus = addedEntity.Entity.ImmigrationStatus,
			SocialInsuranceNumber = addedEntity.Entity.SocialInsuranceNumber,
			UniqueClientIdentifier = addedEntity.Entity.UniqueClientIdentifier
		};
	}

	public async Task<bool> DeleteAsync(int biographicalDataId)
	{
		int affected = 0;
		var dataInDb = await _context.BiographicalDatas.FindAsync(biographicalDataId);
		if (dataInDb is not null)
		{
			_context.BiographicalDatas.Remove(dataInDb);
			affected = await _context.SaveChangesAsync();
		}

		if (affected == 0)
			return false;
		return true;
	}

	public async Task<ICollection<BiographicalData>?> GetAllAsync()
	{
		var result =
			from biographicalData in _context.BiographicalDatas
			select new BiographicalData
			{
				Id = biographicalData.Id,
				FirstName = biographicalData.FirstName,
				LastName = biographicalData.LastName,
				Email = biographicalData.Email,
				PreferredPronouns = biographicalData.PreferredPronouns,
				LevelOfStudy = biographicalData.LevelOfStudy,
				ImmigrationStatus = biographicalData.ImmigrationStatus,
				SocialInsuranceNumber = biographicalData.SocialInsuranceNumber,
				UniqueClientIdentifier = biographicalData.UniqueClientIdentifier
			};
		return await result.ToListAsync();
	}

	public async Task<BiographicalData?> GetAsync(int biographicalDataId)
	{
		var result =
			from biographicalData in _context.BiographicalDatas
			select new BiographicalData
			{
				Id = biographicalData.Id,
				FirstName = biographicalData.FirstName,
				LastName = biographicalData.LastName,
				Email = biographicalData.Email,
				PreferredPronouns = biographicalData.PreferredPronouns,
				LevelOfStudy = biographicalData.LevelOfStudy,
				ImmigrationStatus = biographicalData.ImmigrationStatus,
				SocialInsuranceNumber = biographicalData.SocialInsuranceNumber,
				UniqueClientIdentifier = biographicalData.UniqueClientIdentifier
			};
		return await result.FirstOrDefaultAsync();
	}

	public async Task<bool> UpdateAsync(BiographicalData biographicalData)
	{
		if (biographicalData is null)
			throw new NullReferenceException(SqlBiographicalDataRepositoryErrors.BiographicalData_IsNull);

		var dataInDb = await _context.BiographicalDatas.FindAsync(biographicalData.Id);
		if (dataInDb is null)
			return false;

		_mapper.MapToEntityByRef(biographicalData, dataInDb);
		_context.Update(dataInDb);

		int rowsAffected = await _context.SaveChangesAsync();

		if (rowsAffected == 0)
			return false;
		return true;
	}
}