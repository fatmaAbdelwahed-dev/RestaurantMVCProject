using RestaurantProject.Models;

namespace RestaurantProject.Services
{
    public interface IFoodItemService
    {
        Task<List<FoodItem>> GetAllFoodItemsAsync();

        Task<List<FoodItem>> GetAvailableFoodItemsAsync();

        Task<FoodItem?> GetFoodItemByIdAsync(int id);

        Task<FoodItem?> GetAvailableFoodItemByIdAsync(int id);

        Task CreateFoodItemAsync(FoodItem foodItem);

        Task UpdateFoodItemAsync(FoodItem foodItem);

        Task DeleteFoodItemAsync(int id);

        bool FoodItemExists(int id);
    }
}