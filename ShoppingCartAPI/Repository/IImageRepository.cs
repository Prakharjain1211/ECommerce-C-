using ShoppingCartAPI.Models;

namespace ShoppingCartAPI.Repository
{
    public interface IImageRepository
    {
         Task<ProductImage> Upload(IFormFile file, ProductImage productImage);

        Task<IEnumerable<ProductImage>> GetAll();
    }
}