namespace RestaurantOrders.Domain.Interfaces.Services;

/// <summary>Notifica participantes do fluxo de pedido em tempo real (Supabase Realtime).</summary>
public interface INotificationService
{
    /// <summary>Avisa a cozinha sobre um novo pedido recebido.</summary>
    Task NotifyKitchenAsync(Guid restaurantId, Guid orderId, CancellationToken ct = default);

    /// <summary>Envia uma mensagem para o app do cliente na mesa.</summary>
    Task NotifyTableAsync(Guid tableId, string message, CancellationToken ct = default);
}
