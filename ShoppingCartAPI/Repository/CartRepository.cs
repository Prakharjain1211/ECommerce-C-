using Microsoft.EntityFrameworkCore;
using ShoppingCartAPI.Data;
using ShoppingCartAPI.Models;

namespace ShoppingCartAPI.Repository
{
    public class CartRepository : ICartRepository
    {
        private readonly ApplicationDbContext _context;

        public CartRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // public async Task<Cart> GetCartByUserIdAsync(string userId)
        // {
        //     return await _context.Carts
        //         .Include(c => c.CartItems)
        //         .FirstOrDefaultAsync(c => c.UserId == userId);
        // }

        // public async Task AddCartAsync(Cart cart)
        // {
        //     await _context.Carts.AddAsync(cart);
        //     await _context.SaveChangesAsync();
        // }

        // public async Task AddCartItemAsync(CartItem cartItem)
        // {
        //     await _context.CartItems.AddAsync(cartItem);
        //     await _context.SaveChangesAsync();
        // }

        // public async Task RemoveCartItemAsync(int cartItemId, string userId)
        // {
        //     var cartItem = await _context.CartItems
        //         .Include(ci => ci.Cart)
        //         .FirstOrDefaultAsync(ci => ci.Id == cartItemId && ci.Cart.UserId == userId);

        //     if (cartItem != null)
        //     {
        //         _context.CartItems.Remove(cartItem);
        //         await _context.SaveChangesAsync();
        //    
        //  }
        // }        

        public async Task<Cart> GetCartByUserId(string userId)
        {
            return await _context.Carts
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync(c => c.UserId == userId);
        }

        public async Task AddCartItem(int cartId, CartItem cartItem)
        {
            var cart = await _context.Carts.FindAsync(cartId);
            if (cart != null)
            {
                var existingItem = cart.CartItems.FirstOrDefault(ci => ci.ProductId == cartItem.ProductId);
                if (existingItem != null)
                {
                    existingItem.Quantity += cartItem.Quantity;
                }
                else
                {
                    cart.CartItems.Add(cartItem);
                }
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateCartItem(CartItem cartItem)
        {
            _context.CartItems.Update(cartItem);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveCartItem(int cartId, int productId)
        {
            var cart = await _context.Carts.FindAsync(cartId);
            if (cart != null)
            {
                var cartItem = cart.CartItems.FirstOrDefault(ci => ci.ProductId == productId);
                if (cartItem != null)
                {
                    cart.CartItems.Remove(cartItem);
                    await _context.SaveChangesAsync();
                }
            }
        }

        public async Task CreateCart(Cart cart) 
        {
            await _context.Carts.AddAsync(cart);
            await _context.SaveChangesAsync();
        }

        public async Task ClearCart(string userId)
        {
            var cart = await GetCartByUserId(userId);
            if (cart != null)
            {
                _context.CartItems.RemoveRange(cart.CartItems);
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateCart(Cart cart)
        {
            _context.Carts.Update(cart);
            await _context.SaveChangesAsync();
        }
    }
}
