using AsifNutsNSeeds.Models;

namespace AsifNutsNSeeds.Data.Services
{
	public interface IOrdersService
	{
		Task StoreOrderAsync(List<ShoppingCartItem> items, string userid, string userEmailAddress);
		Task<List<Order>> GetOrdersByUserIdAndRoleAsync(string userId, string userRole);
	}
}
