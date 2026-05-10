namespace RestaurantOrders.Application.Orders.Commands.CreateOrder;

using MediatR;

/// <summary>
/// Command to create a new order
/// </summary>
public class CreateOrderCommand : IRequest<Guid>
{
    public Guid RestaurantId { get; set; }
    public Guid TableId { get; set; }
    public Guid? CustomerId { get; set; }
}
