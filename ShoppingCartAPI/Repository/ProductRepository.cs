using Microsoft.EntityFrameworkCore;
using ShoppingCartAPI.Data;
using ShoppingCartAPI.Models;

namespace ShoppingCartAPI.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Product> AddAsync(Product product)
        {
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _context.Products.Include(x => x.Categories).ToListAsync();
        }


        public async Task<Product> GetByIdAsync(int id)
        {
            return await _context.Products.Include(x => x.Categories).FirstOrDefaultAsync(x => x.Id == id);
        }


        // public async Task<IEnumerable<Product>> GetProductsByCategoryAsync(string category)
        // {
        //     return await _context.Products
        //         .Where(p => p.Categories.Any(c => c.Name.Equals(category, StringComparison.OrdinalIgnoreCase)))
        //         .ToListAsync();
        // }

        public async Task<IEnumerable<Product>> GetProductsByCategoryAsync(string category)
        {
            var categoryLower = category.ToLower();

            return await _context.Products
                .Where(p => p.Categories.Any(c => c.Name.ToLower() == categoryLower))
                .ToListAsync();
        }

        public async Task<Product?> UpdateAsync(Product product)
        {
            var existingProduct = await _context.Products.Include(x => x.Categories).FirstOrDefaultAsync(x => x.Id == product.Id);
            if (existingProduct == null)
            {
                return null;
            }
            _context.Entry(existingProduct).CurrentValues.SetValues(product);
            existingProduct.Categories = product.Categories;
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task<Product> DeleteAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
                return product;
            }
            return null;
        }
    }
}
