namespace RestaurantOrders.Application.Orders.Commands.AddItemToOrder;

using MediatR;

/// <summary>
/// Command to add an item to an order
/// </summary>
public class AddItemToOrderCommand : IRequest<Unit>
{
    public Guid OrderId { get; set; }
    public Guid MenuItemId { get; set; }
    public int Quantity { get; set; }
    public string? Notes { get; set; }
}
