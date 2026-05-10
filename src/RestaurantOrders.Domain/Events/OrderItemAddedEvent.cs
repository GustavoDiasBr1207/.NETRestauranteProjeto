namespace RestaurantOrders.Domain.Events;

using RestaurantOrders.Domain.Common;

/// <summary>
/// Domain event raised when an item is added to an order
/// </summary>
public record OrderItemAddedEvent(
    Guid OrderId,
    Guid MenuItemId,
    int Quantity
) : IDomainEvent;
