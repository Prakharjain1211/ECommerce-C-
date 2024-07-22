namespace ShoppingCartAPI.DTO
{
    public class ProductDto
    {
         public int Id { get; set; }

        public string Name { get; set; }

        public string? ShortDescription { get; set; }

        public decimal Price { get; set; }

        public string? FeaturedImageUrl { get; set; }

        public List<ProductCategoryDto> Categories { get; set; } = new List<ProductCategoryDto>();
   
    }
}