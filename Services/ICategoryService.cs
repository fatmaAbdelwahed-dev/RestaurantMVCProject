using RestaurantProject.Models;

namespace RestaurantProject.Services
{
    public interface ICategoryService
    {
        Task<List<Category>> GetAllCategoriesAsync();

        Task<Category?> GetCategoryByIdAsync(int id);

        Task CreateCategoryAsync(Category category);

        Task UpdateCategoryAsync(Category category);

        Task DeleteCategoryAsync(int id);

        bool CategoryExists(int id);

        Task<bool> CategoryNameExistsAsync(string name);

        Task<bool> CategoryNameExistsForOtherCategoryAsync(string name, int categoryId);
    }
}