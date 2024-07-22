using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShoppingCartAPI.DTO;
using ShoppingCartAPI.Models;
using ShoppingCartAPI.Repository;
using System.Security.Claims;

namespace ShoppingAPI.Controllers
{
    // [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class CartController : ControllerBase
    {
        private readonly ICartRepository _cartRepository;

        public CartController(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }

        [HttpGet("{userId}")]
        // [HttpGet]
        public async Task<ActionResult<CartDto>> GetCart(string userId)
        {
            // var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var cart = await _cartRepository.GetCartByUserId(userId);
            if (cart == null)
            {
                return NotFound();
            }


            return Ok(cart);
        }

        [HttpPost("{userId}/items")]
        public async Task<ActionResult> AddCartItem(string userId, [FromBody] AddCartItemDto cartItemDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var cart = await _cartRepository.GetCartByUserId(userId);
            if (cart == null)
            {
                var user = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (user == null)
                {
                    return NotFound();
                }

                cart = new Cart { UserId = userId, CartItems = new List<CartItem>() };
                await _cartRepository.CreateCart(cart);
            }

            var cartItem = cart.CartItems.FirstOrDefault(ci => ci.ProductId == cartItemDTO.ProductId);
            if (cartItem != null)
            {
                cartItem.Quantity += cartItemDTO.Quantity;
            }
            else
            {
                cart.CartItems.Add(new CartItem
                {
                    CartId = cart.Id,
                    ProductId = cartItemDTO.ProductId,
                    Quantity = cartItemDTO.Quantity
                });
            }

            await _cartRepository.UpdateCart(cart);

            return Ok();

        }

        [HttpDelete("{userId}/items/{productId}")]
        public async Task<ActionResult> RemoveCartItem(string userId, int productId)
        {
            var cart = await _cartRepository.GetCartByUserId(userId);
            if (cart == null)
            {
                return NotFound();
            }

            await _cartRepository.RemoveCartItem(cart.Id, productId);
            return NoContent();
        }

        [HttpPost("{userId}/items/{productId}/increment")]
        public async Task<ActionResult> IncrementCartItem(string userId, int productId)
        {
            var cart = await _cartRepository.GetCartByUserId(userId);
            if (cart == null)
            {
                return NotFound();
            }

            var cartItem = cart.CartItems.FirstOrDefault(ci => ci.ProductId == productId);
            if (cartItem == null)
            {
                return NotFound();
            }

            cartItem.Quantity++;
            await _cartRepository.UpdateCartItem(cartItem);

            return NoContent();
        }

        [HttpPost("{userId}/items/{productId}/decrement")]
        public async Task<ActionResult> DecrementCartItem(string userId, int productId)
        {
            var cart = await _cartRepository.GetCartByUserId(userId);
            if (cart == null)
            {
                return NotFound();
            }

            var cartItem = cart.CartItems.FirstOrDefault(ci => ci.ProductId == productId);
            if (cartItem == null)
            {
                return NotFound();
            }

            if (cartItem.Quantity > 1)
            {
                cartItem.Quantity--;
                await _cartRepository.UpdateCartItem(cartItem);
            }
            else
            {
                await _cartRepository.RemoveCartItem(cart.Id, productId);
            }

            return NoContent();
        }

        [HttpPost("{userId}/merge")]
        public async Task<ActionResult> MergeCart(string userId, [FromBody] CartDto localCart)
        {
            var cart = await _cartRepository.GetCartByUserId(userId);
            if (cart == null)
            {
                cart = new Cart { UserId = userId, CartItems = new List<CartItem>() };
                await _cartRepository.CreateCart(cart);
            }

            foreach (var localCartItem in localCart.CartItems)
            {
                var cartItem = cart.CartItems.FirstOrDefault(ci => ci.ProductId == localCartItem.ProductId);
                if (cartItem != null)
                {
                    cartItem.Quantity += localCartItem.Quantity;
                }
                else
                {
                    cart.CartItems.Add(new CartItem
                    {
                        CartId = cart.Id,
                        ProductId = localCartItem.ProductId,
                        Quantity = localCartItem.Quantity,
                        Product = new Product // Ensure you fetch the actual product details if needed
                        {
                            Id = localCartItem.Product.Id,
                            Name = localCartItem.Product.Name,
                            Price = localCartItem.Product.Price,
                            Categories = (ICollection<ProductCategory>)localCartItem.Product.Categories.Select(c => new ProductCategoryDto{
                                Id = c.Id,
                                Name = c.Name,
                            })
                        }
                    });
                }
            }

            await _cartRepository.UpdateCart(cart);

            return Ok();
        }
    }
}