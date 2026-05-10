namespace RestaurantOrders.Domain.Events;

using RestaurantOrders.Domain.Common;
using RestaurantOrders.Domain.Enums;

/// <summary>
/// Domain event raised when order status changes
/// </summary>
public record OrderStatusChangedEvent(
    Guid OrderId,
    OrderStatusEnum OldStatus,
    OrderStatusEnum NewStatus
) : IDomainEvent;
