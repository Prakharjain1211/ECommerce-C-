using ShoppingCartAPI.Models;

namespace ShoppingCartAPI.Repository
{
    public interface IProductCategoryRepository
    {
        Task<IEnumerable<ProductCategory>> GetAllAsync();
        Task<ProductCategory> GetByIdAsync(int id);

        //new changes
        //IEnumerable<Product> SearchProductsByName(string name);

        Task AddAsync(ProductCategory productCategory);
        Task<ProductCategory?> UpdateAsync(ProductCategory productCategory);
        Task DeleteAsync(int id);
    }
}
