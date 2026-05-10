namespace RestaurantOrders.API.Middleware;

/// <summary>
/// Middleware for handling exceptions and returning ProblemDetails
/// </summary>
public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    
    public ExceptionHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }
    
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception exception)
        {
            // TODO: Implement exception handling and logging
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            
            var response = new { message = exception.Message };
            await context.Response.WriteAsJsonAsync(response);
        }
    }
}
