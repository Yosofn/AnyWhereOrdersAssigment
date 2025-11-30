using Microsoft.EntityFrameworkCore;
using Orders.Domain.Entities;
using Orders.Domain.IRepositories;
using Orders.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orders.Infrastructure.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly AppDbContext _db;
        public OrderRepository(AppDbContext db) => _db = db;

        public async Task<Order> CreateAsync(Order order)
        {
            _db.Orders.Add(order);
            await _db.SaveChangesAsync();
            return order;
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await _db.Orders.FindAsync(id);
            if (entity != null)
            {
                _db.Orders.Remove(entity);
                await _db.SaveChangesAsync();
            }
        }

        public async Task<Order?> GetByIdAsync(Guid id)
        {
            return await _db.Orders.AsNoTracking().FirstOrDefaultAsync(o => o.OrderId == id);
        }

        public async Task<IReadOnlyList<Order>> ListAsync()
        {
            return await _db.Orders.AsNoTracking().OrderByDescending(o => o.CreatedAt).ToListAsync();
        }
    }
}
