namespace RestaurantOrders.Application.Common.Behaviors;

using MediatR;

/// <summary>
/// Pipeline behavior for validation using FluentValidation
/// </summary>
public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    public Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        // FluentValidation registration will handle validation automatically
        return next();
    }
}
