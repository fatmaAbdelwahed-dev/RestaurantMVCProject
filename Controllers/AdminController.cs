using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantProject.Services;

namespace RestaurantProject.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly IFoodItemService _foodItemService;
        private readonly IOrderService _orderService;

        public AdminController(
            ICategoryService categoryService,
            IFoodItemService foodItemService,
            IOrderService orderService)
        {
            _categoryService = categoryService;
            _foodItemService = foodItemService;
            _orderService = orderService;
        }

        public async Task<IActionResult> Dashboard()
        {
            var categories = await _categoryService.GetAllCategoriesAsync();
            var foodItems = await _foodItemService.GetAllFoodItemsAsync();

            ViewBag.CategoriesCount = categories.Count;
            ViewBag.FoodItemsCount = foodItems.Count;
            ViewBag.AvailableFoodItemsCount = foodItems.Count(f => f.IsAvailable);

            ViewBag.OrdersCount = await _orderService.GetOrdersCountAsync();
            ViewBag.PendingOrdersCount = await _orderService.GetPendingOrdersCountAsync();
            ViewBag.DeliveredOrdersCount = await _orderService.GetDeliveredOrdersCountAsync();
            ViewBag.TotalSales = await _orderService.GetTotalSalesAsync();

            var latestOrders = await _orderService.GetLatestOrdersAsync(5);

            return View(latestOrders);
        }
    }
}