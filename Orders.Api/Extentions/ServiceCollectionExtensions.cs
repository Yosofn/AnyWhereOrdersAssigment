using Microsoft.EntityFrameworkCore;
using Orders.Application.Services;
using Orders.Domain.IRepositories;
using Orders.Infrastructure.Cashe;
using Orders.Infrastructure.Context;
using Orders.Infrastructure.Repositories;
using StackExchange.Redis;

namespace Orders.Api.Extentions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, string connectionString, string redisConnection)
        {
            // DbContext
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(connectionString));

            // Repositories
            services.AddScoped<IOrderRepository, OrderRepository>();

            // Redis
            services.AddSingleton<IConnectionMultiplexer>(sp =>
                ConnectionMultiplexer.Connect(redisConnection));

            services.AddScoped<ICashService, RedisCacheService>();

            return services;
        }

        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            // Application services
            services.AddScoped<OrderService>();

            return services;
        }
    }
}

