
using Orders.Api.Extentions;
using Orders.Api.MiddleWare;
using Orders.Infrastructure.Cashe;
using StackExchange.Redis;

namespace Orders.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Connection strings
            var sqlConnection = builder.Configuration.GetConnectionString("DefaultConnection");
            var redisConnection = builder.Configuration.GetConnectionString("Redis");

            // Register layers
            builder.Services.AddInfrastructure(sqlConnection!, redisConnection!);
            builder.Services.AddApplication();
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
          

            var app = builder.Build();
            app.UseGlobalExceptionHandling();
            app.UseRequestLogging();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
