namespace RestaurantOrders.Application.Orders.Commands.CancelOrder;

using MediatR;

/// <summary>
/// Command to cancel an order
/// </summary>
public class CancelOrderCommand : IRequest<Unit>
{
    public Guid OrderId { get; set; }
}
