using ShoppingCartAPI.Models;

namespace ShoppingCartAPI.Repository
{
    public interface IProductRepository
    {
        Task<Product> AddAsync(Product product);

        Task<IEnumerable<Product>> GetAllAsync();
        Task<Product> GetByIdAsync(int id);


       
          Task<IEnumerable<Product>> GetProductsByCategoryAsync(string category);
        
        Task<Product?> UpdateAsync(Product product);
        Task<Product?> DeleteAsync(int id);
    }
}
