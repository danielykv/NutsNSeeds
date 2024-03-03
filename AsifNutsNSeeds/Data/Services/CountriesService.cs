using AsifNutsNSeeds.Data.Base;
using AsifNutsNSeeds.Models;

namespace AsifNutsNSeeds.Data.Services
{
	public class CountriesService : EntityBaseRepository<Country>, ICountriesService
	{
		private readonly AppDbContext _context;

		public CountriesService(AppDbContext context) : base(context) { }
	}
}
