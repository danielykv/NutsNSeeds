﻿using AsifNutsNSeeds.Data;
using AsifNutsNSeeds.Data.Services;
using AsifNutsNSeeds.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AsifNutsNSeeds.Controllers
{
    public class ProducersController : Controller
    {
        private readonly IProducersService _service;
        public ProducersController(IProducersService service)
        {
            _service = service;
        }

        //Handle deafult page for producers with list of producers


        public async Task<IActionResult> Index()
        {
            var allProducers = await _service.GetAllAsync();
            return View(allProducers);
        }

        //GET: producers/details/1

        public async Task<IActionResult> Details(int id)
        {
            var producerDetails = await _service.GetByIdAsync(id);
			if (producerDetails == null) return View("NotFound");
			return View(producerDetails);
		}
		//GET: producers/create
		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Create( Producer producer)
		{
			if (!ModelState.IsValid)
			{
				return View(producer);
			}
			await _service.AddAsync(producer);
			return RedirectToAction(nameof(Index));

		}

		//GET: producers/edit/1
		public async Task<IActionResult> Edit(int id)
		{
			var producerDetails = await _service.GetByIdAsync(id);
			if (producerDetails == null) return View("NotFound");
			return View(producerDetails);
		}

		[HttpPost]
		public async Task<IActionResult> Edit(int id, Producer producer)
		{
			if (!ModelState.IsValid) return View(producer);

			if (id == producer.Id)
			{
				await _service.UpdateAsync(id, producer);
				return RedirectToAction(nameof(Index));
			}
			return View(producer);
		}

		//GET: producers/delete/1
		public async Task<IActionResult> Delete(int id)
		{
			var producerDetails = await _service.GetByIdAsync(id);
			if (producerDetails == null) return View("NotFound");
			return View(producerDetails);
		}

		[HttpPost, ActionName("Delete")]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			var producerDetails = await _service.GetByIdAsync(id);
			if (producerDetails == null) return View("NotFound");

			await _service.DeleteAsync(id);
			return RedirectToAction(nameof(Index));
		}

	}
}
