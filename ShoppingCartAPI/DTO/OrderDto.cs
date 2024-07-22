using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShoppingCartAPI.Models;

namespace ShoppingCartAPI.DTO
{
   public class OrderDTO
    {
        public int Id { get; set; }
        public string? UserId { get; set; }

      
        public DateTime OrderDate { get; set; }

        
        public List<OrderItemDTO>? OrderItems { get; set; }

    
        public decimal TotalAmount { get; set; }
    }

public class OrderItemDTO
{
    public int Id { get; set; }
        public int OrderId { get; set; }
        public Order? Order { get; set; }
        public int ProductId { get; set; }
        public Product? Product { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }

        public string? ProductName{get; set; }
        public string? ProductUrl {get; set; }
}
}