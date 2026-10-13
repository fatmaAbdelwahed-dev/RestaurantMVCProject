using Microsoft.EntityFrameworkCore;
using RestaurantProject.Data;
using RestaurantProject.Models;

namespace RestaurantProject.Services
{
    public class FoodItemService : IFoodItemService
    {
        private readonly ApplicationDbContext _context;

        public FoodItemService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<FoodItem>> GetAllFoodItemsAsync()
        {
            return await _context.FoodItems
                .Include(f => f.Category)
                .ToListAsync();
        }

        public async Task<List<FoodItem>> GetAvailableFoodItemsAsync()
        {
            return await _context.FoodItems
                .Include(f => f.Category)
                .Where(f => f.IsAvailable)
                .ToListAsync();
        }

        public async Task<FoodItem?> GetFoodItemByIdAsync(int id)
        {
            return await _context.FoodItems
                .Include(f => f.Category)
                .FirstOrDefaultAsync(f => f.FoodItemId == id);
        }

        public async Task<FoodItem?> GetAvailableFoodItemByIdAsync(int id)
        {
            return await _context.FoodItems
                .Include(f => f.Category)
                .FirstOrDefaultAsync(f => f.FoodItemId == id && f.IsAvailable);
        }

        public async Task CreateFoodItemAsync(FoodItem foodItem)
        {
            _context.FoodItems.Add(foodItem);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateFoodItemAsync(FoodItem foodItem)
        {
            _context.FoodItems.Update(foodItem);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteFoodItemAsync(int id)
        {
            var foodItem = await _context.FoodItems.FindAsync(id);

            if (foodItem != null)
            {
                _context.FoodItems.Remove(foodItem);
                await _context.SaveChangesAsync();
            }
        }

        public bool FoodItemExists(int id)
        {
            return _context.FoodItems.Any(f => f.FoodItemId == id);
        }
    }
}