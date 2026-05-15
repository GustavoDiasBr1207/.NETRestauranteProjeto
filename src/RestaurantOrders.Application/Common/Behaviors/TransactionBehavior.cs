namespace RestaurantOrders.Application.Common.Behaviors;

using MediatR;

// Transações são gerenciadas pelo SaveChangesAsync do ApplicationDbContext.
// Para transações cross-aggregate, injete IUnitOfWork e use BeginTransactionAsync.
public class TransactionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    public Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken ct)
        => next();
}
