namespace RestaurantOrders.Application.Orders.EventHandlers;

using MediatR;
using RestaurantOrders.Domain.Events;

/// <summary>
/// Event handler for OrderPlacedEvent
/// </summary>
public class OrderPlacedEventHandler : INotificationHandler<OrderPlacedEvent>
{
    public Task Handle(OrderPlacedEvent notification, CancellationToken cancellationToken)
    {
        // TODO: Implement event handling logic
        return Task.CompletedTask;
    }
}
