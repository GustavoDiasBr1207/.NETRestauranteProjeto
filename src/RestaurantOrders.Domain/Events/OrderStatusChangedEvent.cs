namespace RestaurantOrders.Domain.Events;

using RestaurantOrders.Domain.Common;
using RestaurantOrders.Domain.Enums;

/// <summary>Publicado a cada transição de status do pedido.</summary>
public record OrderStatusChangedEvent(
    Guid            OrderId,
    OrderStatusEnum OldStatus,
    OrderStatusEnum NewStatus
) : IDomainEvent;
