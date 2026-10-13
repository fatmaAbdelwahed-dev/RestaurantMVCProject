using System.ComponentModel.DataAnnotations;

namespace RestaurantProject.Models
{
    public class Category
    {
        public int CategoryId { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(200)]
        public string? Description { get; set; }

        public ICollection<FoodItem>? FoodItems { get; set; }
    }
}