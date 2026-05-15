namespace RestaurantOrders.Application.Orders.EventHandlers;

using MediatR;
using Microsoft.Extensions.Logging;
using RestaurantOrders.Domain.Events;

/// <summary>
/// Registra em log cada transição de status do pedido.
/// Estenda este handler para notificar o cliente via Supabase Realtime quando necessário.
/// </summary>
public class OrderStatusChangedEventHandler(ILogger<OrderStatusChangedEventHandler> logger)
    : INotificationHandler<OrderStatusChangedEvent>
{
    public Task Handle(OrderStatusChangedEvent notification, CancellationToken ct)
    {
        logger.LogInformation(
            "Pedido {OrderId}: status alterado de {StatusAnterior} para {StatusNovo}.",
            notification.OrderId,
            notification.OldStatus,
            notification.NewStatus);

        return Task.CompletedTask;
    }
}
