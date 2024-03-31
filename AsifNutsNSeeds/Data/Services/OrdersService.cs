using AsifNutsNSeeds.Models;
using Microsoft.EntityFrameworkCore;

namespace AsifNutsNSeeds.Data.Services
{
	public class OrdersService : IOrdersService
	{
		private readonly AppDbContext _context;

        public OrdersService(AppDbContext context)
        {
			_context = context;

		}
        public async Task<List<Order>> GetOrdersByUserIdAndRoleAsync(string userId, string userRole)
		{
			var orders = await _context.Orders.Include(n =>n.OrderItems).ThenInclude(n => n.product).Include(n=> n.User).ToListAsync();
			
			if(userRole != "Admin")
			{
				orders = orders.Where(n => n.UserId == userId).ToList();
			}
			return orders;
		}

		public async Task StoreOrderAsync(List<ShoppingCartItem> items, string userid, string userEmailAddress)
		{
			var order = new Order() { UserId = userid, Email = userEmailAddress };
			await _context.Orders.AddAsync(order);
			await _context.SaveChangesAsync();

            foreach (var item in items)
            {
				var orderItem = new OrderItem()
				{
					Amount = item.Amount,
					ProductId = item.Product.Id,
					OrderId = order.OrderId,
					Price = item.Product.ProductPrice


				};
				await _context.OrderItems.AddAsync(orderItem);
            }
			await _context.SaveChangesAsync();
        }

	}
}
