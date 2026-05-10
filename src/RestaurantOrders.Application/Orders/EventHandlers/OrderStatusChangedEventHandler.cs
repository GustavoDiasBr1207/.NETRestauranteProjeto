namespace RestaurantOrders.Application.Orders.EventHandlers;

using MediatR;
using RestaurantOrders.Domain.Events;

/// <summary>
/// Event handler for OrderStatusChangedEvent
/// </summary>
public class OrderStatusChangedEventHandler : INotificationHandler<OrderStatusChangedEvent>
{
    public Task Handle(OrderStatusChangedEvent notification, CancellationToken cancellationToken)
    {
        // TODO: Implement event handling logic
        return Task.CompletedTask;
    }
}
