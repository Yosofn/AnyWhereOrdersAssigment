using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orders.Application.DTOs
{
    public record CreateOrderDto(string CustomerName, string Product, decimal Amount);
    public record OrderDto(Guid OrderId, string CustomerName, string Product, decimal Amount, DateTime CreatedAt);
}
