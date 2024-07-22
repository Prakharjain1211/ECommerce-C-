using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ShoppingCartAPI.Models
{
    public class Cart
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }  // Identity user ID

        public ICollection<CartItem> CartItems { get; set; }
    }

    public class CartItem
    {
        public int Id { get; set; }

        [Required]
        public int CartId { get; set; }
        [JsonIgnore]
        public Cart Cart { get; set; }

        [Required]
        public int ProductId { get; set; }

    //new
        public Product Product { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
        public int Quantity { get; set; }
    }
}
