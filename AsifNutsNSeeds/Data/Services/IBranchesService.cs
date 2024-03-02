using AsifNutsNSeeds.Models;
using System.Collections.Generic;
namespace AsifNutsNSeeds.Data.Services
{
    public interface IBranchesService
    {
        Task<IEnumerable<Branch>> GetAllAsync();
        Task<Branch> GetByIdAsync(int id);
        Task AddAsync(Branch branch);
        Task<Branch>  UpdateAsync(int id, Branch newBranch);
        Task DeleteAsync(int id);
    }
}
