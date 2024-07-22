// using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
// using MimeKit;
using ShoppingCartAPI.Data;
using ShoppingCartAPI.DTO;
using ShoppingCartAPI.Models;
using ShoppingCartAPI.Repository;
using System.Security.Claims;


namespace ShoppingCartAPI.Controllers
{
    // [Authorize]
    [ApiController]
    [Route("api/[controller]")]
   
    public class OrderController : ControllerBase
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ICartRepository _cartRepository;

        public OrderController(IOrderRepository orderRepository, ICartRepository cartRepository)
        {
            _orderRepository = orderRepository;
            _cartRepository = cartRepository;
        }

        [HttpGet]
        //[Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<OrderDTO>>> GetAllOrders()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var orders = await _orderRepository.GetAllOrders();
            var orderDTOs = orders.Select(order => new OrderDTO
            {
                Id = order.Id,
                UserId = userId,
                OrderDate = order.OrderDate,
                TotalAmount = order.TotalAmount,
                OrderItems = order.OrderItems.Select(oi => new OrderItemDTO
                {
                    ProductId = oi.ProductId,
                    Quantity = oi.Quantity,
                    UnitPrice = oi.UnitPrice,
                    // new addition
                    // Product = oi.Product,
                    ProductName=oi.Product.Name,
                    ProductUrl = oi.Product.FeaturedImageUrl
                }).ToList()
            }).ToList();

            return Ok(orderDTOs);
        }

        [HttpGet("{id}")]
       // [Authorize(Roles = "Admin")]
        public async Task<ActionResult<OrderDTO>> GetOrderById(int id)
        {
            var order = await _orderRepository.GetOrderById(id);
            if (order == null)
            {
                return NotFound();
            }

            var orderDTO = new OrderDTO
            {
                Id = order.Id,
                UserId = order.UserId,
                OrderDate = order.OrderDate,
                TotalAmount = order.TotalAmount,
                OrderItems = order.OrderItems.Select(oi => new OrderItemDTO
                {
                    ProductId = oi.ProductId,
                    Quantity = oi.Quantity,
                    UnitPrice = oi.UnitPrice,
                    ProductName = oi.Product.Name,
                    ProductUrl = oi.Product.FeaturedImageUrl
                }).ToList()
            };

            return Ok(orderDTO);
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<OrderDTO>>> GetOrdersByUserId(string userId)
        {
            var orders = await _orderRepository.GetOrdersByUserId(userId);
            if (orders == null || !orders.Any())
            {
                return NotFound("No orders found for this user.");
            }

            var orderDTOs = orders.Select(order => new OrderDTO
            {
                Id = order.Id,
                UserId = order.UserId,
                OrderDate = order.OrderDate,
                TotalAmount = order.TotalAmount,
                OrderItems = order.OrderItems.Select(oi => new OrderItemDTO
                {
                    ProductId = oi.ProductId,
                    Quantity = oi.Quantity,
                    UnitPrice = oi.UnitPrice,
                    ProductName = oi.Product.Name,
                    ProductUrl = oi.Product.FeaturedImageUrl
                }).ToList()
            }).ToList();

            return Ok(orderDTOs);
        }

        [HttpPost("{userId}")]
        //[Authorize(Roles = "User")]
        public async Task<ActionResult> PlaceOrder(string userId)
        {
            var cart = await _cartRepository.GetCartByUserId(userId);
            if (cart == null || !cart.CartItems.Any())
            {
                return BadRequest("The cart is empty or does not exist.");
            }

            var order = new Order
            {
                UserId = userId,
                OrderDate = DateTime.Now,
                TotalAmount = cart.CartItems.Sum(ci => ci.Quantity * ci.Product.Price),
                OrderItems = cart.CartItems.Select(ci => new OrderItem
                {
                    ProductId = ci.ProductId,
                    Quantity = ci.Quantity,
                    UnitPrice = ci.Product.Price
                }).ToList()
            };

            await _orderRepository.AddOrder(order);

            // Clear the cart after placing the order
            /* cart.CartItems.Clear();
             await _cartRepository.RemoveCartItem(cart.Id, 0);*/
            await _cartRepository.ClearCart(userId);

            return Ok(new { Message = "Order placed successfully" });
        }
    }

}
