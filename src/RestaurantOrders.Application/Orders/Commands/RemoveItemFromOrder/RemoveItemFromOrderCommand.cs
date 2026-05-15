namespace RestaurantOrders.Application.Orders.Commands.RemoveItemFromOrder;

using MediatR;

public class RemoveItemFromOrderCommand : IRequest
{
    public Guid OrderId     { get; set; }
    public Guid OrderItemId { get; set; }
}
