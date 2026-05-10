namespace RestaurantOrders.Application.Orders.Commands.RemoveItemFromOrder;

using MediatR;
using RestaurantOrders.Domain.Interfaces.Repositories;
using RestaurantOrders.Domain.Exceptions;

/// <summary>
/// Handler for RemoveItemFromOrderCommand
/// </summary>
public class RemoveItemFromOrderCommandHandler : IRequestHandler<RemoveItemFromOrderCommand>
{
    private readonly IOrderRepository _orderRepository;
    
    public RemoveItemFromOrderCommandHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }
    
    public async Task Handle(RemoveItemFromOrderCommand request, CancellationToken cancellationToken)
    {
        // TODO: Implement logic to remove item from order
        var order = await _orderRepository.GetByIdWithItemsAsync(request.OrderId, cancellationToken)
            ?? throw new NotFoundException($"Order with id '{request.OrderId}' not found");
        
        order.RemoveItem(request.OrderItemId);
        await _orderRepository.UpdateAsync(order, cancellationToken);
    }
}
