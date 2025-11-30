using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Orders.Application.DTOs;
using Orders.Application.Services;
using Orders.Domain.Entities;
using Orders.Infrastructure.Cashe;

namespace Orders.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly OrderService _orderService;
        private readonly ICashService _cache;

        public OrdersController(OrderService orderService, ICashService cache)
        {
            _orderService = orderService;
            _cache = cache;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDto dto)
        {
            var order = await _orderService.CreateOrderAsync(dto);
            await _cache.SetAsync($"order:{order.OrderId}", order, TimeSpan.FromMinutes(5));
            await _cache.RemoveAsync("orders:list");

            return CreatedAtAction(nameof(GetOrderById),
                new { id = order.OrderId },
                new ApiResponse<Order>(order, "Order created successfully"));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(Guid id)
        {
            var cached = await _cache.GetAsync<Order>($"order:{id}");
            if (cached != null)
            {
                return Ok(new ApiResponse<Order>(cached, "Order retrieved from cache"));
            }
            var order = await _orderService.GetOrderByIdAsync(id);
            if (order == null)
                return NotFound(ApiResponse<Order>.Fail("Order not found"));

            await _cache.SetAsync($"order:{id}", order, TimeSpan.FromMinutes(5));


            return Ok(new ApiResponse<Order>(order, "Order retrieved successfully"));
        }

        [HttpGet]
        public async Task<IActionResult> ListOrders()
        {
            const string cacheKey = "orders:list";

            var cachedOrders = await _cache.GetAsync<IEnumerable<Order>>(cacheKey);
            if (cachedOrders != null)
                return Ok(new ApiResponse<IEnumerable<Order>>(cachedOrders, "Orders list retrieved from cache"));

            var orders = await _orderService.ListOrdersAsync();

            await _cache.SetAsync(cacheKey, orders, TimeSpan.FromMinutes(1));

            return Ok(new ApiResponse<IEnumerable<Order>>(orders, "Orders list retrieved"));
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(Guid id)
        {
            await _orderService.DeleteOrderAsync(id);
            await _cache.RemoveAsync($"order:{id}");
            await _cache.RemoveAsync("orders:list");
            return Ok(new ApiResponse<string>(null, "Order deleted successfully"));
        }
    }
}