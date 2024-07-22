using System.ComponentModel.DataAnnotations;

namespace ShoppingCartAPI.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        public string? ShortDescription { get; set; }

        [Required]
        [Range(0.01, 10000.00)]
        public decimal Price { get; set; }

        public string? FeaturedImageUrl { get; set; }

        public ICollection<ProductCategory> Categories { get; set; }
    }
}
