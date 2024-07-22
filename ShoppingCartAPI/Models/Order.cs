using System.ComponentModel.DataAnnotations;

namespace ShoppingCartAPI.Models
{
    public class Order
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }  // Identity user ID

        [Required]
        public DateTime OrderDate { get; set; }

        // public User User { get; set; }

        public List<OrderItem> OrderItems { get; set; }

        public decimal TotalAmount{ get; set; }
    }

    public class OrderItem
    {
        public int Id { get; set; }

        [Required]
        public int OrderId { get; set; }
        public Order Order { get; set; }

        [Required]
        public int ProductId { get; set; }

        public Product Product { get; set; }
        
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
        public int Quantity { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
        public decimal UnitPrice { get; set; }
    }
}