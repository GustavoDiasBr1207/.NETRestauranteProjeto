namespace RestaurantOrders.API.Middleware;

/// <summary>
/// Middleware for logging requests and responses
/// </summary>
public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;
    
    public RequestLoggingMiddleware(RequestDelegate next)
    {
        _next = next;
    }
    
    public async Task InvokeAsync(HttpContext context)
    {
        // TODO: Implement request logging (method, path, status, duration)
        await _next(context);
    }
}
