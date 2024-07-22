using ShoppingCartAPI.Models;

namespace ShoppingCartAPI.Repository
{
    public interface IOrderRepository
    {
        Task<IEnumerable<Order>> GetAllOrders();
        Task<Order> GetOrderById(int id);
        Task AddOrder(Order order);
        Task<IEnumerable<Order>> GetOrdersByUserId(string userId);
    }
}
