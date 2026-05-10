namespace RestaurantOrders.Domain.Events;

using RestaurantOrders.Domain.Common;
using RestaurantOrders.Domain.ValueObjects;

/// <summary>
/// Domain event raised when an order is placed
/// </summary>
public record OrderPlacedEvent(
    Guid OrderId,
    Guid RestaurantId,
    Guid TableId,
    Money TotalAmount
) : IDomainEvent;
