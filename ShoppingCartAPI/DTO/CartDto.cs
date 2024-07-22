using System.ComponentModel.DataAnnotations;
namespace ShoppingCartAPI.DTO
{
    public class CartDto
    {
        public int Id { get; set; }

        public string UserId { get; set; }        

        public List<CartItemDto> CartItems { get; set; }
    }

    public class CartItemDto
    {

        [Required]
        public int ProductId { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
        public int Quantity { get; set; }

        public ProductDto Product { get; set; }
    }
}
