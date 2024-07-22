using Microsoft.EntityFrameworkCore;
using ShoppingCartAPI.Data;
using ShoppingCartAPI.Models;

namespace ShoppingCartAPI.Repository
{
    public class ProductCategoryRepository : IProductCategoryRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductCategoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ProductCategory>> GetAllAsync()
        {
            return await _context.ProductCategories.ToListAsync();
        }

        public async Task<ProductCategory> GetByIdAsync(int id)
        {
            return await _context.ProductCategories.FindAsync(id);
        }

        public async Task AddAsync(ProductCategory productCategory)
        {
            _context.ProductCategories.Add(productCategory);
            await _context.SaveChangesAsync();
        }

        public async Task<ProductCategory?> UpdateAsync(ProductCategory productCategory)
        {
            var existingCategory = await _context.ProductCategories.FirstOrDefaultAsync(x => x.Id == productCategory.Id);
            if(existingCategory != null)
            {
                // _context.ProductCategories.Update(productCategory);
                _context.Entry(existingCategory).CurrentValues.SetValues(productCategory);
                await _context.SaveChangesAsync();
                return productCategory;
            }
            return null;
        }

        public async Task DeleteAsync(int id)
        {
            var productCategory = await _context.ProductCategories.FindAsync(id);
            if (productCategory != null)
            {
                _context.ProductCategories.Remove(productCategory);
                await _context.SaveChangesAsync();
            }
        }
    }

}
