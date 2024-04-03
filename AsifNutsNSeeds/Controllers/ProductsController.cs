using AsifNutsNSeeds.Data;
using AsifNutsNSeeds.Data.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace AsifNutsNSeeds.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductsService _service;
        public ProductsController(IProductsService service)
        {
			_service = service;
        }

        public async Task<IActionResult> Index()
        {
            var allProducts = await _service.GetAllAsync(n => n.Country);
            return View(allProducts);
        }

        public async Task<IActionResult> Filter(string searchString)
        {
            var allProducts = await _service.GetAllAsync(n => n.Country);

            if (!string.IsNullOrEmpty(searchString))
            {
                var filteredResult = allProducts.Where(n =>
                    n.ProductName.ToLower().Contains(searchString.ToLower()) ||
                    n.ProductDescription.ToLower().Contains(searchString.ToLower())
                ).ToList();

                return View("Index", filteredResult);
            }

            return View("Index", allProducts);
        }


        //GET: Movies/Details/1
        [AllowAnonymous]
		public async Task<IActionResult> Details(int id)
		{
			var productDetail = await _service.GetProductByIdAsync(id);
			return View(productDetail);
		}

        //GET: Movies/Create
        public async Task<IActionResult> Create()
        {
            var productDropdownsData = await _service.GetNewProductDropdownsValues();

            ViewBag.Countries = new SelectList(productDropdownsData.Countries, "Id", "CountryName");
            ViewBag.Producers = new SelectList(productDropdownsData.Producers, "Id", "ProducerName");
            ViewBag.Branches = new SelectList(productDropdownsData.Branches, "Id", "BranchName");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(NewProductVM product)
        {
            if (!ModelState.IsValid)
            {
                var productDropdownsData = await _service.GetNewProductDropdownsValues();

                ViewBag.Cinemas = new SelectList(productDropdownsData.Countries, "Id", "CountryName");
                ViewBag.Producers = new SelectList(productDropdownsData.Producers, "Id", "ProducerName");
                ViewBag.Actors = new SelectList(productDropdownsData.Branches, "Id", "BranchName");

                return View(product);
            }

            await _service.AddNewProductAsync(product);
            return RedirectToAction(nameof(Index));
        }

        //GET: Products/Edit/1
        public async Task<IActionResult> Edit(int id)
        {
            var productDetails = await _service.GetProductByIdAsync(id);
            if (productDetails == null) return View("NotFound");

            var response = new NewProductVM()
            {
                Id = productDetails.Id,
                ProductName = productDetails.ProductName,
                ProductDescription = productDetails.ProductDescription,
                ProductPrice = productDetails.ProductPrice,
                ImageURL = productDetails.ImageURL,
                ProductCategory = productDetails.productCategory,
                CountryID = productDetails.CountryID,
                ProducerID = productDetails.ProducerID,
                BranchIds = productDetails.Product_Branches.Select(n => n.Id).ToList(),
            };

            var productDropdownsData = await _service.GetNewProductDropdownsValues();
            ViewBag.Countries = new SelectList(productDropdownsData.Countries, "Id", "CountryName");
            ViewBag.Producers = new SelectList(productDropdownsData.Producers, "Id", "ProducerName");
            ViewBag.Branches = new SelectList(productDropdownsData.Branches, "Id", "BranchName");

            return View(response);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, NewProductVM product)
        {
            if (id != product.Id) return View("NotFound");

            if (!ModelState.IsValid)
            {
                var productDropdownsData = await _service.GetNewProductDropdownsValues();

                ViewBag.Countries = new SelectList(productDropdownsData.Countries, "Id", "CountryName");
                ViewBag.Producers = new SelectList(productDropdownsData.Producers, "Id", "ProducerName");
                ViewBag.Branches= new SelectList(productDropdownsData.Branches, "Id", "BranchName");

                return View(product);
            }

            await _service.UpdateProductAsync(product);
            return RedirectToAction(nameof(Index));
        }

    }
}
