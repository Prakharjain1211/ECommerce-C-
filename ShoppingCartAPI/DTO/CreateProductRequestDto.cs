namespace ShoppingCartAPI.DTO
{
    public class CreateProductRequestDto
    {


        public string Name { get; set; }

        public string? ShortDescription { get; set; }

        public decimal Price { get; set; }

        public string? FeaturedImageUrl { get; set; }
        public int[] Categories { get; set; }
    }
}
