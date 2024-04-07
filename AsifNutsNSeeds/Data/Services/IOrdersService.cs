using AsifNutsNSeeds.Models;

namespace AsifNutsNSeeds.Data.Services
{
	public interface IOrdersService
	{
        Task StoreOrderAsync(List<ShoppingCartItem> items, string userId, string userEmailAddress, string userAddress, string userPostalCode, string userCity, DateTime orderDate);
        Task<List<Order>> GetOrdersByUserIdAndRoleAsync(string userId, string userRole);
	}
}
