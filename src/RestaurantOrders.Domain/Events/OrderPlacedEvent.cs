namespace RestaurantOrders.Domain.Events;

using RestaurantOrders.Domain.Common;
using RestaurantOrders.Domain.ValueObjects;

/// <summary>Publicado quando o cliente envia o pedido para a cozinha (Draft → Pending).</summary>
public record OrderPlacedEvent(
    Guid  OrderId,
    Guid  RestaurantId,
    Guid  TableId,
    Money TotalAmount
) : IDomainEvent;
