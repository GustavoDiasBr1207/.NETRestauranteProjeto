namespace RestaurantOrders.Application.Common.Behaviors;

using MediatR;
using RestaurantOrders.Domain.Interfaces;

/// <summary>
/// Ponto único de commit: chama <see cref="IUnitOfWork.CommitAsync"/> ao final de cada
/// command, garantindo atomicidade entre todos os repositórios do mesmo escopo.
/// Queries não geram mudanças rastreadas, então o commit é no-op para elas.
/// </summary>
public class TransactionBehavior<TRequest, TResponse>(IUnitOfWork unitOfWork)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken ct)
    {
        var response = await next();
        await unitOfWork.CommitAsync(ct);
        return response;
    }
}
