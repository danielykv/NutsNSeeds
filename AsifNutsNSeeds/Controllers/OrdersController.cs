using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using AsifNutsNSeeds.Data;
using AsifNutsNSeeds.Data.Cart;
using AsifNutsNSeeds.Data.Services;
using AsifNutsNSeeds.Data.ViewModels;
using AsifNutsNSeeds.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace AsifNutsNSeeds.Controllers
{
    public class OrdersController : Controller
    {
        private readonly IProductsService _productsService;
        private readonly ShoppingCart _shoppingCart;
        private readonly IOrdersService _ordersService;
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public OrdersController(IProductsService productsService, ShoppingCart shoppingCart, IOrdersService ordersService, AppDbContext context, IConfiguration configuration)
        {
            _productsService = productsService;
            _shoppingCart = shoppingCart;
            _ordersService = ordersService;
            _context = context;
            _configuration = configuration;
        }
        public async  Task<IActionResult> Index()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            string userRole = User.FindFirstValue(ClaimTypes.Role); 
            var orders = await _ordersService.GetOrdersByUserIdAndRoleAsync(userId,userRole);

            return View(orders);
        }
        public IActionResult ShoppingCart()
        {
            var items = _shoppingCart.GetShoppingCartItems();
            _shoppingCart.ShoppingCartItems = items;
            var response = new ShoppingCartVM()
            {
                ShoppingCart = _shoppingCart,
                ShoppingCartTotal = _shoppingCart.GetShoppingCartTotal()
            };
            return View(response);
        }
        public async Task<RedirectToActionResult> BuyItemNow(int Id)
        {
            var item = await _productsService.GetProductByIdAsync(Id);
            if (item != null && item.Stock > 0)
            {
                _shoppingCart.AddItemToCart(item);
            }

          

            return RedirectToAction(nameof(ShoppingCart));
        }

        


        public async Task<RedirectToActionResult> RemoveItemFromShoppingCart(int Id)
        {
            var item = await _productsService.GetProductByIdAsync(Id);
            if (item != null)
            {
                _shoppingCart.RemoveItemFromCart(item);
            }
            return RedirectToAction(nameof(ShoppingCart));
        }
        public async Task<IActionResult> CompleteOrder()
        {
            var items = _shoppingCart.GetShoppingCartItems();
            string userId = User?.FindFirstValue(ClaimTypes.NameIdentifier) ?? "AnonymousID";
            string userEmailAddress = User?.FindFirstValue(ClaimTypes.Email) ?? "AnonymousEmail";

            // Retrieve user information from the database based on the user ID
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);

            // Check if user exists and retrieve address, city, and postal code
            string userAddress = user?.Address;
            string userCity = user?.City;
            string userPostalCode = user?.PostalCode;

            // Set the order date to the current date
            DateTime orderDate = DateTime.Now;

            // Store the order
            await _ordersService.StoreOrderAsync(items, userId, userEmailAddress, userAddress, userCity, userPostalCode, orderDate);

            // Clear the shopping cart
            await _shoppingCart.ClearShoppingCartAsync();

            // Decrement stock for each product in the order
            foreach (var item in items)
            {
                var product = await _context.Products.FindAsync(item.Product.Id);
                if (product != null)
                {
                    // Ensure stock doesn't go negative
                    product.Stock = Math.Max(0, product.Stock - 1);
                    product.Sold = product.Sold + 1;
                }
            }

            // Save changes to the database
            await _context.SaveChangesAsync();

            int orderNumber = await _context.Orders
                                .OrderByDescending(o => o.OrderId)
                                .Select(o => o.OrderId)
                                .FirstOrDefaultAsync();

            var smtpSettings = _configuration.GetSection("SmtpSettings");
            var host = smtpSettings["Host"];
            var port = int.Parse(smtpSettings["Port"]);
            var enableSSL = bool.Parse(smtpSettings["EnableSSL"]);
            var username = smtpSettings["Username"];
            var password = smtpSettings["Password"];

            var message = new MailMessage();
            message.From = new MailAddress("etomer9@gmail.com"); // Set the sender's email address
            message.To.Add(userEmailAddress);
            message.Subject = "Order Completed";
            message.Body = $"Dear {userEmailAddress},\n\nThank you for purchasing!\n\nOrder number: {orderNumber}\n\nTThank you.";

            using (var smtp = new SmtpClient(host, port))
            {
                smtp.EnableSsl = enableSSL;
                smtp.Credentials = new NetworkCredential(username, password);

                await smtp.SendMailAsync(message);
            }

            return View("OrderCompleted");
        }

        public async Task<IActionResult> CompleteOrderByGuest(string Address, string City, string PostalCode, string Email)

        {

            var items = _shoppingCart.GetShoppingCartItems();
            string userId = "0a8a3652-c22a-4fd3-af73-12f410aedca3";
            string userEmailAddress = Email;

            // Retrieve user information from the database based on the user ID

            // Check if user exists and retrieve address, city, and postal code
            string userAddress = Address;
            string userCity = City;
            string userPostalCode = PostalCode;

            // Set the order date to the current date
            DateTime orderDate = DateTime.Now;

            // Store the order
            await _ordersService.StoreOrderAsync(items, userId, userEmailAddress, userAddress, userCity, userPostalCode, orderDate);

            // Clear the shopping cart
            await _shoppingCart.ClearShoppingCartAsync();

            // Decrement stock for each product in the order
            foreach (var item in items)
            {
                var product = await _context.Products.FindAsync(item.Product.Id);
                if (product != null)
                {
                    // Ensure stock doesn't go negative
                    product.Stock = Math.Max(0, product.Stock - 1);
                    product.Sold = product.Sold + 1;
                }
            }

            // Save changes to the database
            await _context.SaveChangesAsync();

            int orderNumber = await _context.Orders
                                .OrderByDescending(o => o.OrderId)
                                .Select(o => o.OrderId)
                                .FirstOrDefaultAsync();


            var smtpSettings = _configuration.GetSection("SmtpSettings");
            var host = smtpSettings["Host"];
            var port = int.Parse(smtpSettings["Port"]);
            var enableSSL = bool.Parse(smtpSettings["EnableSSL"]);
            var username = smtpSettings["Username"];
            var password = smtpSettings["Password"];

            var message = new MailMessage();
            message.From = new MailAddress("etomer9@gmail.com"); // Set the sender's email address
            message.To.Add(userEmailAddress);
            message.Subject = "Order Completed";
            message.Body = $"Dear {userEmailAddress},\n\nThank you for purchasing!\n\nOrder number: {orderNumber}\n\nTThank you.";

            using (var smtp = new SmtpClient(host, port))
            {
                smtp.EnableSsl = enableSSL;
                smtp.Credentials = new NetworkCredential(username, password);

                await smtp.SendMailAsync(message);
            }

            return View("OrderCompleted");
        }


    }
}
