using AsifNutsNSeeds.Data.Base;
using AsifNutsNSeeds.Models;
using Microsoft.EntityFrameworkCore;

namespace AsifNutsNSeeds.Data.Services
{
    public class BranchesService : EntityBaseRepository<Branch>, IBranchesService
    {
        private readonly AppDbContext _context;

        public BranchesService(AppDbContext context): base(context) { }
      
    }
}
