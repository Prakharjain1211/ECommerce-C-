using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingCartAPI.Models
{
    public class ProductImage
    {
        public Guid Id { get; set; }
        public string FileName { get; set; }
        public string FileExtension { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public DateTime DateCreated { get; set; }
    }
}