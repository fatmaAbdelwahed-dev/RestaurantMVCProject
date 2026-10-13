using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RestaurantProject.Models;
using RestaurantProject.Services;

namespace RestaurantProject.Controllers
{
    [Authorize]
    public class OrdersController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly UserManager<ApplicationUser> _userManager;

        public OrdersController(
            IOrderService orderService,
            UserManager<ApplicationUser> userManager)
        {
            _orderService = orderService;
            _userManager = userManager;
        }

        // GET: Orders/Create?foodItemId=5
        public async Task<IActionResult> Create(int foodItemId)
        {
            var foodItem = await _orderService.GetFoodItemForOrderAsync(foodItemId);

            if (foodItem == null)
            {
                return NotFound();
            }

            var model = new CreateOrderViewModel
            {
                FoodItemId = foodItem.FoodItemId,
                FoodName = foodItem.Name,
                Price = foodItem.Price,
                ImageUrl = foodItem.ImageUrl,
                Quantity = 1
            };
            return View(model);
        }

        // POST: Orders/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateOrderViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            try
            {
                await _orderService.CreateOrderAsync(model, user.Id);
            }
            catch
            {
                return NotFound();
            }

            return RedirectToAction(nameof(MyOrders));
        }

        // GET: Orders/MyOrders
        public async Task<IActionResult> MyOrders()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var orders = await _orderService.GetOrdersByUserIdAsync(user.Id);

            return View(orders);
        }

        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);
            var order = await _orderService.GetOrderDetailsAsync(id.Value);

            if (order == null)
            {
                return NotFound();
            }

            if (!User.IsInRole("Admin") && order.UserId != user?.Id)
            {
                return Forbid();
            }

            return View(order);
        }

        // GET: Orders/AdminOrders
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AdminOrders()
        {
            var orders = await _orderService.GetAllOrdersAsync();
            return View(orders);
        }

        // POST: Orders/UpdateStatus
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStatus(int orderId, string status)
        {
            await _orderService.UpdateOrderStatusAsync(orderId, status);
            return RedirectToAction(nameof(AdminOrders));
        }
    }
}