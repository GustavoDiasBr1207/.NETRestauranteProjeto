namespace RestaurantOrders.Application.Common.Behaviors;

using MediatR;

/// <summary>
/// Pipeline behavior for transaction management
/// </summary>
public class TransactionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        // TODO: Implement transaction handling
        var response = await next();
        return response;
    }
}
