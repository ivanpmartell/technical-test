namespace BiographicalDetails.Domain.Abstractions
{
	public interface IBiographicalDataRepository
	{
		public Task<ICollection<BiographicalData>?> GetAllAsync();
		public Task<BiographicalData?> GetAsync(int biographicalDataId);
		public Task<BiographicalData?> AddAsync(BiographicalData biographicalData);
		public Task<bool> UpdateAsync(BiographicalData biographicalData);
		public Task<bool> DeleteAsync(int biographicalDataId);
	}
}
