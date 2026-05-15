namespace RestaurantOrders.Application.Orders.Commands.SubmitOrder;

using MediatR;

public class SubmitOrderCommand : IRequest
{
    public Guid OrderId { get; set; }
}
