using System.Diagnostics.Metrics;
using AsifNutsNSeeds.Data;
using AsifNutsNSeeds.Data.Services;
using AsifNutsNSeeds.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AsifNutsNSeeds.Controllers
{
    public class CountriesController : Controller
    {

		private readonly ICountriesService _service;
		public CountriesController(ICountriesService service)
		{
			_service = service;
		}

		public async Task<IActionResult> Index()
        {
            var allCountries = await _service.GetAllAsync();
            return View(allCountries);
        }

		//GET: Countries/details/1

		public async Task<IActionResult> Details(int id)
		{
			var countryDetails = await _service.GetByIdAsync(id);
			if (countryDetails == null) return View("NotFound");
			return View(countryDetails);
		}
		//GET: Countries/create
		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Create(Country country)
		{
			if (!ModelState.IsValid)
			{
				return View(country);
			}
			await _service.AddAsync(country);
			return RedirectToAction(nameof(Index));

		}

		//GET: Countries/edit/1
		public async Task<IActionResult> Edit(int id)
		{
			var countryDetails = await _service.GetByIdAsync(id);
			if (countryDetails == null) return View("NotFound");
			return View(countryDetails);
		}

		[HttpPost]
		public async Task<IActionResult> Edit(int id, Country country)
		{
			if (!ModelState.IsValid) return View(country);

			if (id == country.Id)
			{
				await _service.UpdateAsync(id, country);
				return RedirectToAction(nameof(Index));
			}
			return View(country);
		}

		//GET: Countries/delete/1
		public async Task<IActionResult> Delete(int id)
		{
			var countryDetails = await _service.GetByIdAsync(id);
			if (countryDetails == null) return View("NotFound");
			return View(countryDetails);
		}

		[HttpPost, ActionName("Delete")]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			var countryDetails = await _service.GetByIdAsync(id);
			if (countryDetails == null) return View("NotFound");

			await _service.DeleteAsync(id);
			return RedirectToAction(nameof(Index));
		}
	}
}
