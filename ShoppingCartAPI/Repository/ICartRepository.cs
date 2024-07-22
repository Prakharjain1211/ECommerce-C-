using ShoppingCartAPI.Models;

namespace ShoppingCartAPI.Repository
{
    public interface ICartRepository
    {
        // Task<Cart> GetCartByUserIdAsync(string userId);
        // Task AddCartAsync(Cart cart);
        // Task AddCartItemAsync(CartItem cartItem);
        // Task RemoveCartItemAsync(int cartItemId, string userId);

        Task<Cart> GetCartByUserId(string userId);
        Task AddCartItem(int cartId, CartItem cartItem);

        Task UpdateCartItem(CartItem cartItem);
        Task RemoveCartItem(int cartId, int productId);
        Task CreateCart(Cart cart);
        Task ClearCart(string userId);

        Task UpdateCart(Cart cart);
    }
}
