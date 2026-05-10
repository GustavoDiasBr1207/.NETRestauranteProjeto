namespace RestaurantOrders.Application.Common.Behaviors;

using MediatR;
using System.Diagnostics;

/// <summary>
/// Pipeline behavior for logging request/response and execution time
/// </summary>
public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var stopwatch = Stopwatch.StartNew();
        
        // TODO: Implement logging
        
        var response = await next();
        
        stopwatch.Stop();
        
        // TODO: Log response and execution time
        
        return response;
    }
}
