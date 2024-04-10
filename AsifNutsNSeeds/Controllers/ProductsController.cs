using AsifNutsNSeeds.Data;
using AsifNutsNSeeds.Data.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Net.Mail;
using System.Net;
using System.Security.Claims;
using AsifNutsNSeeds.Models;
using Microsoft.Data.SqlClient;
using AsifNutsNSeeds.Data.Enums;
using System.Linq.Expressions;
using AsifNutsNSeeds.Data.Cart;
using Microsoft.AspNetCore.Http;

namespace AsifNutsNSeeds.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductsService _service;
        private readonly IConfiguration _configuration;
        private readonly ShoppingCart _shoppingCart;

        private readonly AppDbContext _context;

        public ProductsController(IProductsService service, AppDbContext context, IConfiguration configuration, ShoppingCart shoppingCart)
        {
			_service = service;
            _context = context;
            _configuration = configuration;
            _shoppingCart = shoppingCart;

        }

        //Handle deafult page for produts with list of branches

        public async Task<IActionResult> Index(int page = 1, int pageSize = 8)
        {
            var allProducts = await _service.GetAllAsync(n => n.Country);
            string userId = User?.FindFirstValue(ClaimTypes.NameIdentifier) ?? "AnonymousID";
            var paginatedProducts = PaginatedList<Product>.Create(allProducts.ToList(), page, pageSize);

            ViewData["FilterQueryString"] = "";

            // Checking if notification email for any user is needed.

            // Get the list of users who have subscribed to notifications for products that are back in stock
            var productsToNotify = await _context.ProductNotifications
                  .Select(pn => new { pn.ProductId, pn.UserId })
                  .Distinct()
                  .ToListAsync();
        
            var userToNotify = await _context.ProductNotifications
              .Select(pn => pn.UserId)
              .Distinct()
              .ToListAsync();


            foreach (var notifyProduct in productsToNotify)
            {
                foreach (var prod in allProducts)
                {
                    if (prod.Id == notifyProduct.ProductId && prod.Stock > 0)
                    {
                        var userObj = await _context.Users.FirstOrDefaultAsync(u => u.Id == notifyProduct.UserId);

                        var userEmail = userObj.Email;

                        var smtpSettings = _configuration.GetSection("SmtpSettings");
                        var host = smtpSettings["Host"];
                        var port = int.Parse(smtpSettings["Port"]);
                        var enableSSL = bool.Parse(smtpSettings["EnableSSL"]);
                        var username = smtpSettings["Username"];
                        var password = smtpSettings["Password"];

                        var message = new MailMessage();
                        message.From = new MailAddress("etomer9@gmail.com"); // Set the sender's email address
                        message.To.Add(userEmail);
                        message.Subject = "Product Back in Stock Notification";
                        message.Body = $"Dear {userEmail},\n\nOne or more products you subscribed to are now back in stock.\n\nThank you.";

                        using (var smtp = new SmtpClient(host, port))
                        {
                            smtp.EnableSsl = enableSSL;
                            smtp.Credentials = new NetworkCredential(username, password);

                            await smtp.SendMailAsync(message);
                        }
                        // Remove the corresponding notification from the database
                        var notificationToRemove = await _context.ProductNotifications
                            .FirstOrDefaultAsync(pn => pn.ProductId == notifyProduct.ProductId && pn.UserId == notifyProduct.UserId);

                        if (notificationToRemove != null)
                        {
                            _context.ProductNotifications.Remove(notificationToRemove);
                            await _context.SaveChangesAsync();
                        }
                    }
                }

            }

            return View(paginatedProducts);
        }

        //Handle the functionality of notify button on product that is not in stock

        public async Task<RedirectToActionResult> Notify(int Id)
        {
            var item = await _service.GetProductByIdAsync(Id);
            string userId = User?.FindFirstValue(ClaimTypes.NameIdentifier);

            try
            {
                // Check if a notification already exists for the same product and user
                var existingNotification = await _context.ProductNotifications
                    .FirstOrDefaultAsync(n => n.ProductId == item.Id && n.UserId == userId);

                // Check if the product is out of stock and was just restocked
                if (item.Stock <= 0 && existingNotification == null)
                {
                    var newNotification = new ProductNotification
                    {
                        ProductId = item.Id,
                        UserId = userId,
                    };
                    // Add the new notification to the context
                    _context.ProductNotifications.Add(newNotification);

                    // Save changes to the database
                    await _context.SaveChangesAsync();
                    TempData["AlertMessage"] = "We'll email you as soon as the product will be back to stock!";
                }
                else
                {
                    TempData["AlertMessage"] = "Already subscribed, dont worry, when the product will back to stock we'll let you know:)";

                }

            }
            catch (DbUpdateException ex)
            {
                // Check if the exception indicates a violation of the unique constraint
                if (ex.InnerException is SqlException sqlEx && sqlEx.Number == 2601)
                {
                    // Handle the unique constraint violation (e.g., log the error, notify the user)
                    // For simplicity, we'll just ignore the exception here
                    // You can add your custom handling logic as needed
                }
                else
                {
                    // If it's not a unique constraint violation, re-throw the exception
                    return RedirectToAction(nameof(Index));
                }
            }

            return RedirectToAction(nameof(Index));
        }

        //Handle add to cart action

        public async Task<IActionResult> AddItemToShoppingCart(int Id)
        {
            var allProducts = await _service.GetAllAsync(n => n.Country);

            var item = await _service.GetProductByIdAsync(Id);
            if (item != null && item.Stock > 0)
            {
                _shoppingCart.AddItemToCart(item);
            }
            TempData["ItemMessage"] = "Item added successfuly!";

            return RedirectToAction("Index"); // Redirect to the Index action

        }

        //Handle correct url strings for all filters
        private string GenerateFilterQueryString(ProductCategory? category, string? searchString, double? minPrice, double? maxPrice,string? highToLow,string? lowToHigh, int page,string? popularity)
        {
            var queryString = "";

            if (category.HasValue)
                queryString = $"FilterByCategory?category={(int)category.Value}";

            if (minPrice.HasValue)
                queryString += $"?minPrice={minPrice.Value}";

            if (maxPrice.HasValue)
                queryString += $"&maxPrice={maxPrice.Value}";

            if (searchString != null)
                queryString = $"Filter?searchString={searchString}";

            if (highToLow != null)
                queryString = $"FilterByPriceLowToHigh";

            if (lowToHigh != null)
                queryString = $"FilterByPriceHighToLow";

            if (popularity != null)
                queryString = $"FilterByPopularity";

            return queryString;
        }

        // Handle search filter

        public async Task<IActionResult> Filter(string searchString, int page = 1, int pageSize = 8)
        {
            var allProducts = await _service.GetAllAsync(n => n.Country);

            if (!string.IsNullOrEmpty(searchString))
            {
                var filteredResult = allProducts.Where(n =>
                    n.ProductName.ToLower().Contains(searchString.ToLower())
                ).ToList();


                var paginatedResult = PaginatedList<Product>.Create(filteredResult, page, pageSize);
                var queryString = GenerateFilterQueryString(null, searchString, null, null,null,null, page,null);
                ViewData["FilterQueryString"] = queryString;

                return View("Index", paginatedResult);
            }
            var paginatedAllProducts = PaginatedList<Product>.Create(allProducts.ToList(), page, pageSize);


            return View("Index", paginatedAllProducts);
        }

        // Handle low to high filter

        public async Task<IActionResult> FilterByPriceLowToHigh(int page = 1, int pageSize = 8)
        {
            var allProducts = await _service.GetAllAsync(n => n.Country);

            var filteredResult = allProducts.OrderBy(n => n.ProductPrice).ToList();

            var paginatedResult = PaginatedList<Product>.Create(filteredResult, page, pageSize);

            var queryString = GenerateFilterQueryString(null, null, null, null,null,"string", page,null);
            ViewData["FilterQueryString"] = queryString;

            return View("Index", paginatedResult);
        }

        //Handle price range filter
        public async Task<IActionResult> FilterByPriceRange(double minPrice, double maxPrice, int page = 1, int pageSize = 8)
        {
            var allProducts = await _service.GetAllAsync(n => n.Country);

            var filteredResult = allProducts.Where(n => n.ProductPrice >= minPrice && n.ProductPrice <= maxPrice)
                                             .OrderBy(n => n.ProductPrice)
                                             .ToList();

            var paginatedResult = PaginatedList<Product>.Create(filteredResult, page, pageSize);

            var queryString = GenerateFilterQueryString(null,null, minPrice, maxPrice,null,null, page,null);
            ViewData["FilterQueryString"] = queryString;

            return View("Index", paginatedResult);
        }

        //Handle high to low filter
        public async Task<IActionResult> FilterByPriceHighToLow(int page = 1, int pageSize = 8)
        {
            var allProducts = await _service.GetAllAsync(n => n.Country);

            var filteredResult = allProducts.OrderByDescending(n => n.ProductPrice).ToList();

            var paginatedResult = PaginatedList<Product>.Create(filteredResult, page, pageSize);

            var queryString = GenerateFilterQueryString(null, null, null, null, "string" , null, page,null);
            ViewData["FilterQueryString"] = queryString;

            return View("Index", paginatedResult);
        }

        //Handle popularuty filter
        public async Task<IActionResult> FilterByPopularity(int page = 1, int pageSize = 8)
        {
            var allProducts = await _service.GetAllAsync(n => n.Country);

            var filteredResult = allProducts.OrderByDescending(n => n.Sold).ToList();

            var paginatedResult = PaginatedList<Product>.Create(filteredResult, page, pageSize);


            var queryString = GenerateFilterQueryString(null, null, null, null, null, null, page, "string");

            ViewData["FilterQueryString"] = queryString;

            return View("Index", paginatedResult);
        }

        //Handdle category filter
        public async Task<IActionResult> FilterByCategory(ProductCategory category, int page = 1, int pageSize = 8)
        {
            // Remove p => p.productCategory from the includeProperties list
            var allProducts = await _service.GetAllAsync(includeProperties: new Expression<Func<Product, object>>[] { p => p.Country });

            // Filter products by category
            var filteredProducts = allProducts.Where(p => p.productCategory == category);


            var paginatedResult = PaginatedList<Product>.Create(filteredProducts.ToList(), page, pageSize);
            var queryString = GenerateFilterQueryString(category,null, null, null,null,null, page,null);
            ViewData["FilterQueryString"] = queryString;

            return View("Index", paginatedResult);
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

                ViewBag.Countries = new SelectList(productDropdownsData.Countries, "Id", "CountryName");
                ViewBag.Producers = new SelectList(productDropdownsData.Producers, "Id", "ProducerName");
                ViewBag.Branches = new SelectList(productDropdownsData.Branches, "Id", "BranchName");

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
                Stock = productDetails.Stock,
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

        //GET: Countries/delete/1
        public async Task<IActionResult> Delete(int id)
        {
            var productDetails = await _service.GetByIdAsync(id);
            if (productDetails == null) return View("NotFound");
            return View(productDetails);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var productDetails = await _service.GetByIdAsync(id);
            if (productDetails == null) return View("NotFound");

            await _service.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

    }
}
