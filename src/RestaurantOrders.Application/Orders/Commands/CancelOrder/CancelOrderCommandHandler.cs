namespace RestaurantOrders.Application.Orders.Commands.CancelOrder;

using MediatR;
using RestaurantOrders.Domain.Interfaces.Repositories;
using RestaurantOrders.Domain.Exceptions;

/// <summary>
/// Handler for CancelOrderCommand
/// </summary>
public class CancelOrderCommandHandler : IRequestHandler<CancelOrderCommand>
{
    private readonly IOrderRepository _orderRepository;
    
    public CancelOrderCommandHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }
    
    public async Task Handle(CancelOrderCommand request, CancellationToken cancellationToken)
    {
        // TODO: Implement logic to cancel order
        var order = await _orderRepository.GetByIdWithItemsAsync(request.OrderId, cancellationToken)
            ?? throw new NotFoundException($"Order with id '{request.OrderId}' not found");
        
        order.Cancel();
        await _orderRepository.UpdateAsync(order, cancellationToken);
    }
}
