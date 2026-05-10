namespace RestaurantOrders.Application.Orders.Commands.SubmitOrder;

using MediatR;

/// <summary>
/// Command to submit an order
/// </summary>
public class SubmitOrderCommand : IRequest<Unit>
{
    public Guid OrderId { get; set; }
}
