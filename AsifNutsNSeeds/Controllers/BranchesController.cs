using AsifNutsNSeeds.Data;
using Microsoft.AspNetCore.Mvc;

namespace AsifNutsNSeeds.Controllers
{

    public class BranchesController : Controller
    {
        private readonly AppDbContext _context;
        public BranchesController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var data = _context.Branches.ToList();
            return View();
        }
    }
}
