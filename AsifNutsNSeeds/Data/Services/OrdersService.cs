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
            try
            {
                var ordersQuery = _context.Orders
                    .Include(n => n.OrderItems).ThenInclude(n => n.product)
                    .Include(n => n.User);

                // Ensure the query is executed and materialized into a list
                var orders = await ordersQuery.ToListAsync();

                // Check if orders is null or empty
                if (orders == null || !orders.Any())
                {
                    return new List<Order>(); // Return an empty list if orders is null or empty
                }

                // Check if userRole is not admin
                if (userRole != "Admin")
                {
                    // Filter orders based on the user ID
                    orders = orders.Where(n => n.UserId == userId).ToList();
                }

                return orders;
            }
            catch (Exception ex)
            {
                // Log the exception or handle it accordingly
                Console.WriteLine($"An error occurred while retrieving orders: {ex.Message}");
                return new List<Order>(); // Return an empty list in case of an exception
            }
        }


        public async Task StoreOrderAsync(List<ShoppingCartItem> items, string userId, string userEmailAddress, string userAddress, string userPostalCode, string userCity, DateTime orderDate)
        {
            var order = new Order()
            {
                UserId = userId,
                Email = userEmailAddress,
                Address = userAddress,
                PostalCode = userPostalCode,
                City = userCity,
                OrderDate = orderDate // Assign orderDate to OrderDate property
            };
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
