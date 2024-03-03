using AsifNutsNSeeds.Data.Base;
using AsifNutsNSeeds.Models;

namespace AsifNutsNSeeds.Data.Services
{
	public class ProducersService : EntityBaseRepository<Producer>, IProducersService
	{
        public ProducersService(AppDbContext context) : base(context) 
        {
            
        }
    }
}
