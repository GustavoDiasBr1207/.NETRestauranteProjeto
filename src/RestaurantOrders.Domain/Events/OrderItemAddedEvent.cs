namespace RestaurantOrders.Domain.Events;

using RestaurantOrders.Domain.Common;

/// <summary>Publicado quando um item é adicionado ao carrinho do pedido.</summary>
public record OrderItemAddedEvent(
    Guid OrderId,
    Guid MenuItemId,
    int  Quantity
) : IDomainEvent;
