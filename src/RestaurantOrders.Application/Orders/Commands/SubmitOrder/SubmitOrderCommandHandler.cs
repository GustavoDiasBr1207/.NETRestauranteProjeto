namespace RestaurantOrders.Application.Orders.Commands.SubmitOrder;

using MediatR;
using RestaurantOrders.Domain.Interfaces.Repositories;
using RestaurantOrders.Domain.Interfaces.Services;
using RestaurantOrders.Domain.Exceptions;

/// <summary>
/// Handler for SubmitOrderCommand
/// </summary>
public class SubmitOrderCommandHandler : IRequestHandler<SubmitOrderCommand>
{
    private readonly IOrderRepository _orderRepository;
    private readonly INotificationService _notificationService;
    
    public SubmitOrderCommandHandler(IOrderRepository orderRepository, INotificationService notificationService)
    {
        _orderRepository = orderRepository;
        _notificationService = notificationService;
    }
    
    public async Task Handle(SubmitOrderCommand request, CancellationToken cancellationToken)
    {
        // TODO: Implement logic to submit order
        var order = await _orderRepository.GetByIdWithItemsAsync(request.OrderId, cancellationToken)
            ?? throw new NotFoundException($"Order with id '{request.OrderId}' not found");
        
        order.Submit();
        await _orderRepository.UpdateAsync(order, cancellationToken);
        
        // Notify kitchen
        await _notificationService.NotifyKitchenAsync(order.RestaurantId, order.Id, cancellationToken);
    }
}
