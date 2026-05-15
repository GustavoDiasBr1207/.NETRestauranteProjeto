namespace RestaurantOrders.Application.Orders.EventHandlers;

using MediatR;
using RestaurantOrders.Domain.Events;
using RestaurantOrders.Domain.Interfaces.Services;

/// <summary>
/// Reage ao envio do pedido para a cozinha notificando via Supabase Realtime.
/// Disparado pelo <c>ApplicationDbContext.SaveChangesAsync</c> após o commit.
/// </summary>
public class OrderPlacedEventHandler(INotificationService notificationService)
    : INotificationHandler<OrderPlacedEvent>
{
    public async Task Handle(OrderPlacedEvent notification, CancellationToken ct)
    {
        await notificationService.NotifyKitchenAsync(notification.RestaurantId, notification.OrderId, ct);
    }
}
