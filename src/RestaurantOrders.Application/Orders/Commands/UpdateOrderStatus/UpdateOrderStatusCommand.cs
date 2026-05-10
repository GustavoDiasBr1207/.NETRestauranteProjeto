namespace RestaurantOrders.Application.Orders.Commands.UpdateOrderStatus;

using MediatR;
using RestaurantOrders.Domain.Enums;

/// <summary>
/// Command to update order status
/// </summary>
public class UpdateOrderStatusCommand : IRequest<Unit>
{
    public Guid OrderId { get; set; }
    public OrderStatusEnum NewStatus { get; set; }
}
