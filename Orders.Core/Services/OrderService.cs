using Orders.Application.DTOs;
using Orders.Application.Interfaces;
using Orders.Domain.Entities;
using Orders.Domain.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orders.Application.Services
{
    public class OrderService: IOrderService
    {
        private readonly IOrderRepository _repo;

        public OrderService(IOrderRepository repo)
        {
            _repo = repo;
        }

        public async Task<Order> CreateOrderAsync(CreateOrderDto dto)
        {
            var order = new Order
            {
                OrderId = Guid.NewGuid(),
                CustomerName = dto.CustomerName,
                Product = dto.Product,
                Amount = dto.Amount,
                CreatedAt = DateTime.UtcNow
            };

            return await _repo.CreateAsync(order);
        }

        public async Task<Order?> GetOrderByIdAsync(Guid id)
        {
            return await _repo.GetByIdAsync(id);
        }

        public async Task<IReadOnlyList<Order>> ListOrdersAsync()
        {
            return await _repo.ListAsync();
        }

        public async Task DeleteOrderAsync(Guid id)
        {
            await _repo.DeleteAsync(id);
        }
    }
}
