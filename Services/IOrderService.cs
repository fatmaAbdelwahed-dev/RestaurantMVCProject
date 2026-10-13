using RestaurantProject.Models;

namespace RestaurantProject.Services
{
    public interface IOrderService
    {
        Task<FoodItem?> GetFoodItemForOrderAsync(int foodItemId);

        Task CreateOrderAsync(CreateOrderViewModel model, string userId);

        Task<List<Order>> GetOrdersByUserIdAsync(string userId);

        Task<Order?> GetOrderDetailsAsync(int orderId);

        Task<List<Order>> GetAllOrdersAsync();

        Task<List<Order>> GetLatestOrdersAsync(int count);

        Task<int> GetOrdersCountAsync();

        Task<int> GetPendingOrdersCountAsync();

        Task<int> GetDeliveredOrdersCountAsync();

        Task<decimal> GetTotalSalesAsync();

        Task UpdateOrderStatusAsync(int orderId, string status);
    }
}