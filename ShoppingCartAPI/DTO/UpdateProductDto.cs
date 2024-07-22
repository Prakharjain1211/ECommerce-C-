using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingCartAPI.DTO
{
    public class UpdateProductDto
    {
       
      
        public string Name { get; set; }

        public string? ShortDescription { get; set; }

        public decimal Price { get; set; }

        public string? FeaturedImageUrl { get; set; }
        public List<int> Categories { get; set; } = new List<int>();
    }
}