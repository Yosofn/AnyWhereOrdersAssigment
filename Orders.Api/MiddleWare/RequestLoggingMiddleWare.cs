using System.Diagnostics;

namespace Orders.Api.MiddleWare
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestLoggingMiddleware> _logger;

        public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var stopwatch = Stopwatch.StartNew();

            _logger.LogInformation("Incoming Request: {Method} {Path}",
                context.Request.Method, context.Request.Path);

            await _next(context);

            stopwatch.Stop();

           

            _logger.LogInformation(
                "Completed Request: {Method} {Path} | Time: {Elapsed} ms ",
                context.Request.Method,
                context.Request.Path,
                stopwatch.ElapsedMilliseconds
                
            );
        }
    }

    public static class RequestLoggingMiddlewareExtensions
    {
        public static IApplicationBuilder UseRequestLogging(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RequestLoggingMiddleware>();
        }
    }
}
