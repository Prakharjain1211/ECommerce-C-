using System.ComponentModel.DataAnnotations;
namespace ShoppingCartAPI.DTO
{
    public class ProductCategoryDto
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }
    }
}
