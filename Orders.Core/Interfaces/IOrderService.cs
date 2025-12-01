using Orders.Application.DTOs;
using Orders.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orders.Application.Interfaces
{
    public interface IOrderService
    {

        Task<Order> CreateOrderAsync(CreateOrderDto dto);
        Task<Order?> GetOrderByIdAsync(Guid id);
        Task<IReadOnlyList<Order>> ListOrdersAsync();
        Task DeleteOrderAsync(Guid id);
    }
}
