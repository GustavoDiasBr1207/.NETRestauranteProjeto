namespace RestaurantOrders.Application.Orders.Commands.CancelOrder;

using MediatR;

public class CancelOrderCommand : IRequest
{
    public Guid OrderId { get; set; }
}
