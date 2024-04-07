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

        public async Task<IActionResult> Index()
        {
            var allProducts = await _service.GetAllAsync(n => n.Country);
            string userId = User?.FindFirstValue(ClaimTypes.NameIdentifier) ?? "AnonymousID";
            

            // Get the list of users who have subscribed to notifications for products that are back in stock
            var productsToNotify = await _context.ProductNotifications
                  .Select(pn => new { pn.ProductId, pn.UserId })
                  .Distinct()
                  .ToListAsync();
        
            var userToNotify = await _context.ProductNotifications
              .Select(pn => pn.UserId)
              .Distinct()
              .ToListAsync();

            // Send email notifications to users
            foreach (var notifyProduct in productsToNotify)
            {
                foreach (var prod in allProducts)
                {
                    if(prod.Id == notifyProduct.ProductId && prod.Stock > 0)
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

            return View(allProducts);
        }
        public async Task<RedirectToActionResult> Notify(int Id)
        {
            var item = await _service.GetProductByIdAsync(Id);
            string userId = User?.FindFirstValue(ClaimTypes.NameIdentifier);

            try
            {
                // Check if the product is out of stock and was just restocked
                if (item.Stock <= 0)
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
        public async Task<IActionResult> AddItemToShoppingCart(int Id)
        {
            var allProducts = await _service.GetAllAsync(n => n.Country);

            var item = await _service.GetProductByIdAsync(Id);
            if (item != null && item.Stock > 0)
            {
                _shoppingCart.AddItemToCart(item);
            }
            return View("Index", allProducts);


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

        public async Task<IActionResult> FilterByPriceLowToHigh()
        {
            var allProducts = await _service.GetAllAsync(n => n.Country);

            var filteredResult = allProducts.OrderBy(n => n.ProductPrice).ToList();

            return View("Index", filteredResult);
        }
        public async Task<IActionResult> FilterByPriceRange(double minPrice, double maxPrice)
        {
            var allProducts = await _service.GetAllAsync(n => n.Country);

            var filteredResult = allProducts.Where(n => n.ProductPrice >= minPrice && n.ProductPrice <= maxPrice)
                                             .OrderBy(n => n.ProductPrice)
                                             .ToList();

            return View("Index", filteredResult);
        }

        public async Task<IActionResult> FilterByPriceHighToLow()
        {
            var allProducts = await _service.GetAllAsync(n => n.Country);

            var filteredResult = allProducts.OrderByDescending(n => n.ProductPrice).ToList();

            return View("Index", filteredResult);
        }
        public async Task<IActionResult> FilterByPopularity()
        {
            var allProducts = await _service.GetAllAsync(n => n.Country);

            var filteredResult = allProducts.OrderByDescending(n => n.Sold).ToList();

            return View("Index", filteredResult);
        }
        public async Task<IActionResult> FilterByCategory(ProductCategory category)
        {
            // Remove p => p.productCategory from the includeProperties list
            var allProducts = await _service.GetAllAsync(includeProperties: new Expression<Func<Product, object>>[] { p => p.Country });

            // Filter products by category
            var filteredProducts = allProducts.Where(p => p.productCategory == category);

            return View("Index", filteredProducts);
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

    }
}
