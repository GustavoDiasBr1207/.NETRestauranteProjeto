namespace RestaurantOrders.Infrastructure.Realtime;

using Microsoft.Extensions.Logging;
using RestaurantOrders.Domain.Interfaces.Services;

// As notificações em tempo real acontecem automaticamente via Supabase Realtime
// quando o EF Core persiste mudanças nas tabelas com REPLICA IDENTITY FULL habilitado.
// Implemente aqui o Broadcast channel quando adicionar o supabase-csharp client.
public class SupabaseNotificationService(ILogger<SupabaseNotificationService> logger) : INotificationService
{
    public Task NotifyKitchenAsync(Guid restaurantId, Guid orderId, CancellationToken ct = default)
    {
        logger.LogInformation("Cozinha notificada — RestaurantId: {RestaurantId}, OrderId: {OrderId}", restaurantId, orderId);
        // TODO: await supabaseClient.Channel($"kitchen:{restaurantId}").Send("new_order", new { order_id = orderId })
        return Task.CompletedTask;
    }

    public Task NotifyTableAsync(Guid tableId, string message, CancellationToken ct = default)
    {
        logger.LogInformation("Mesa notificada — TableId: {TableId}, Message: {Message}", tableId, message);
        // TODO: await supabaseClient.Channel($"table:{tableId}").Send("status_update", new { message })
        return Task.CompletedTask;
    }
}
