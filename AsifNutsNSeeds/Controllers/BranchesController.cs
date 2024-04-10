using System.Diagnostics.Metrics;
using AsifNutsNSeeds.Data;
using AsifNutsNSeeds.Data.Services;
using AsifNutsNSeeds.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AsifNutsNSeeds.Controllers
{

    public class BranchesController : Controller
    {
        private readonly IBranchesService _service;
        public BranchesController(IBranchesService service)
        {
            _service = service;
        }

        //Handle deafult page for branches with list of branches

        public async Task<IActionResult> Index()
        {
            var data = await _service.GetAllAsync();
            return View(data);
        }

        //Get: Branches/Create
        public  IActionResult Create()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Branch branch)
        {
            if(!ModelState.IsValid) 
            {
                return View(branch);
            }
           await _service.AddAsync(branch);
            return RedirectToAction(nameof(Index));
        }

        //Get: Branches/Details/
        //

        public async Task<IActionResult> Details(int id)
        {
            var branchDetails = await _service.GetByIdAsync(id);

            if (branchDetails == null) return View("NotFound");
            return View(branchDetails);
        }

		//Get: Branches/Edit
		public async Task<IActionResult> Edit(int id)
		{
			var branchDetails = await _service.GetByIdAsync(id);

			if (branchDetails == null) return View("NotFound");
			return View(branchDetails);
		}

		[HttpPost]
		public async Task<IActionResult> Edit(int id, Branch branch)
		{

            branch.Id = id;
			if (!ModelState.IsValid)
			{
			return View(branch);

			}
				await _service.UpdateAsync(id, branch);
				return RedirectToAction(nameof(Index));

		}

		//Get: Branches/Delete/1
		public async Task<IActionResult> Delete(int id)
		{
			var branchDetails = await _service.GetByIdAsync(id);

			if (branchDetails == null) return View("NotFound");
			return View(branchDetails);
		}

		[HttpPost, ActionName("Delete")]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			var branchDetails = await _service.GetByIdAsync(id);

			if (branchDetails == null) return View("NotFound");

			await _service.DeleteAsync(id);
			return RedirectToAction(nameof(Index));

		}
	}
}
