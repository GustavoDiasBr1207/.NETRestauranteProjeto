namespace RestaurantOrders.Application.Orders.Commands.RemoveItemFromOrder;

using MediatR;

/// <summary>
/// Command to remove an item from an order
/// </summary>
public class RemoveItemFromOrderCommand : IRequest<Unit>
{
    public Guid OrderId { get; set; }
    public Guid OrderItemId { get; set; }
}
