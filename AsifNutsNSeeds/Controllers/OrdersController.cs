using System.Security.Claims;
using AsifNutsNSeeds.Data.Cart;
using AsifNutsNSeeds.Data.Services;
using AsifNutsNSeeds.Data.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AsifNutsNSeeds.Controllers
{
    public class OrdersController : Controller
    {
        private readonly IProductsService _productsService;
        private readonly ShoppingCart _shoppingCart;
        private readonly IOrdersService _ordersService;
        public OrdersController(IProductsService productsService, ShoppingCart shoppingCart, IOrdersService ordersService)
        {
            _productsService = productsService;
            _shoppingCart = shoppingCart;
            _ordersService = ordersService;
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
        public async Task<RedirectToActionResult> AddItemToShoppingCart(int Id)
        {
            var item = await _productsService.GetProductByIdAsync(Id);
            if (item != null)
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
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            string userEmailAddress = User.FindFirstValue(ClaimTypes.Email);
			await _ordersService.StoreOrderAsync(items, userId, userEmailAddress);
            await _shoppingCart.ClearShoppingCartAsync();
            return View("OrderCompleted");
		}

	}
}
