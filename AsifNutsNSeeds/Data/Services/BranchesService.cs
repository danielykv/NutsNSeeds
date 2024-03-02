using AsifNutsNSeeds.Models;
using Microsoft.EntityFrameworkCore;

namespace AsifNutsNSeeds.Data.Services
{
    public class BranchesService : IBranchesService
    {
        private readonly AppDbContext _context;

        public BranchesService(AppDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(Branch branch)
        {
            await _context.Branches.AddAsync(branch);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var result = await _context.Branches.FirstOrDefaultAsync(n=> n.BranchID == id);
            _context.Branches.Remove(result);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Branch>> GetAllAsync()
        {
            var result =  await _context.Branches.ToListAsync();
            return result;
        }

        public async Task<Branch> GetByIdAsync(int id)
        {
            var result = await (_context.Branches.FirstOrDefaultAsync(n => n.BranchID == id));
            return result;  
        }

        public async Task<Branch> UpdateAsync(int id, Branch newBranch)
        {
            _context.Update(newBranch);
            await _context.SaveChangesAsync();
            return newBranch;
        }
    }
}
