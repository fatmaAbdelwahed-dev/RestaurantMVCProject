using System.ComponentModel.DataAnnotations;

namespace RestaurantProject.Models
{
    public class CreateOrderViewModel
    {
        public int FoodItemId { get; set; }

        public string? FoodName { get; set; }

        public decimal Price { get; set; }

        public string? ImageUrl { get; set; }

        [Required]
        [Range(1, 20)]
        public int Quantity { get; set; } = 1;

        [Required]
        [StringLength(250)]
        public string DeliveryAddress { get; set; }

        [StringLength(300)]
        public string? Notes { get; set; }
    }
}