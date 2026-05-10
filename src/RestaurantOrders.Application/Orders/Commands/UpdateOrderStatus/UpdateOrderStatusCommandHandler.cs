namespace RestaurantOrders.Application.Orders.Commands.UpdateOrderStatus;

using MediatR;
using RestaurantOrders.Domain.Interfaces.Repositories;
using RestaurantOrders.Domain.Enums;
using RestaurantOrders.Domain.Exceptions;

/// <summary>
/// Handler for UpdateOrderStatusCommand
/// </summary>
public class UpdateOrderStatusCommandHandler : IRequestHandler<UpdateOrderStatusCommand>
{
    private readonly IOrderRepository _orderRepository;
    
    public UpdateOrderStatusCommandHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }
    
    public async Task Handle(UpdateOrderStatusCommand request, CancellationToken cancellationToken)
    {
        // TODO: Implement logic to update order status
        var order = await _orderRepository.GetByIdWithItemsAsync(request.OrderId, cancellationToken)
            ?? throw new NotFoundException($"Order with id '{request.OrderId}' not found");
        
        switch (request.NewStatus)
        {
            case OrderStatusEnum.Confirmed:
                order.Confirm();
                break;
            case OrderStatusEnum.Preparing:
                // Transition to preparing if confirmed
                break;
            case OrderStatusEnum.Ready:
                order.MarkAsReady();
                break;
            case OrderStatusEnum.Delivered:
                order.Deliver();
                break;
            case OrderStatusEnum.Cancelled:
                order.Cancel();
                break;
            default:
                throw new InvalidOrderStatusException($"Invalid status: {request.NewStatus}");
        }
        
        await _orderRepository.UpdateAsync(order, cancellationToken);
    }
}
