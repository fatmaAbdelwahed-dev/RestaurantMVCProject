using Microsoft.AspNetCore.Mvc;
using RestaurantProject.Services;

namespace RestaurantProject.Controllers
{
    public class MenuController : Controller
    {
        private readonly IFoodItemService _foodItemService;

        public MenuController(IFoodItemService foodItemService)
        {
            _foodItemService = foodItemService;
        }

        // GET: Menu
        public async Task<IActionResult> Index()
        {
            var foodItems = await _foodItemService.GetAvailableFoodItemsAsync();
            return View(foodItems);
        }

        // GET: Menu/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var foodItem = await _foodItemService.GetAvailableFoodItemByIdAsync(id.Value);

            if (foodItem == null)
            {
                return NotFound();
            }

            return View(foodItem);
        }
    }
}