using Microsoft.EntityFrameworkCore;
using RestaurantProject.Data;
using RestaurantProject.Models;

namespace RestaurantProject.Services
{
    public class OrderService : IOrderService
    {
        private readonly ApplicationDbContext _context;

        public OrderService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<FoodItem?> GetFoodItemForOrderAsync(int foodItemId)
        {
            return await _context.FoodItems
                .Include(f => f.Category)
                .FirstOrDefaultAsync(f => f.FoodItemId == foodItemId && f.IsAvailable);
        }

        public async Task CreateOrderAsync(CreateOrderViewModel model, string userId)
        {
            var foodItem = await _context.FoodItems
                .FirstOrDefaultAsync(f => f.FoodItemId == model.FoodItemId && f.IsAvailable);

            if (foodItem == null)
            {
                throw new Exception("Food item not found or not available.");
            }

            var orderItem = new OrderItem
            {
                FoodItemId = foodItem.FoodItemId,
                Quantity = model.Quantity,
                UnitPrice = foodItem.Price,
                SubTotal = foodItem.Price * model.Quantity
            };

            var order = new Order
            {
                UserId = userId,
                OrderDate = DateTime.Now,
                Status = "Pending",
                DeliveryAddress = model.DeliveryAddress,
                Notes = model.Notes,
                TotalAmount = orderItem.SubTotal,
                OrderItems = new List<OrderItem> { orderItem }
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Order>> GetOrdersByUserIdAsync(string userId)
        {
            return await _context.Orders
                .Include(o => o.OrderItems!)
                    .ThenInclude(oi => oi.FoodItem)
                .Where(o => o.UserId == userId)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();
        }

        public async Task<Order?> GetOrderDetailsAsync(int orderId)
        {
            return await _context.Orders
                .Include(o => o.User)
                .Include(o => o.OrderItems!)
                    .ThenInclude(oi => oi.FoodItem)
                .FirstOrDefaultAsync(o => o.OrderId == orderId);
        }

        public async Task<List<Order>> GetAllOrdersAsync()
        {
            return await _context.Orders
                .Include(o => o.User)
                .Include(o => o.OrderItems!)
                    .ThenInclude(oi => oi.FoodItem)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();
        }

        public async Task<List<Order>> GetLatestOrdersAsync(int count)
        {
            return await _context.Orders
                .Include(o => o.User)
                .OrderByDescending(o => o.OrderDate)
                .Take(count)
                .ToListAsync();
        }

        public async Task<int> GetOrdersCountAsync()
        {
            return await _context.Orders.CountAsync();
        }

        public async Task<int> GetPendingOrdersCountAsync()
        {
            return await _context.Orders
                .CountAsync(o => o.Status == "Pending");
        }

        public async Task<int> GetDeliveredOrdersCountAsync()
        {
            return await _context.Orders
                .CountAsync(o => o.Status == "Delivered");
        }

        public async Task<decimal> GetTotalSalesAsync()
        {
            return await _context.Orders
                .Where(o => o.Status == "Delivered")
                .SumAsync(o => (decimal?)o.TotalAmount) ?? 0;
        }

        public async Task UpdateOrderStatusAsync(int orderId, string status)
        {
            var order = await _context.Orders.FindAsync(orderId);

            if (order != null)
            {
                order.Status = status;
                await _context.SaveChangesAsync();
            }
        }
    }
}