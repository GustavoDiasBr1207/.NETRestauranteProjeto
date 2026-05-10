namespace RestaurantOrders.Infrastructure.Realtime;

using RestaurantOrders.Domain.Interfaces.Services;

/// <summary>
/// Notification service implementation using Supabase Realtime
/// </summary>
public class SupabaseNotificationService : INotificationService
{
    public async Task NotifyKitchenAsync(Guid restaurantId, Guid orderId, CancellationToken cancellationToken = default)
    {
        // TODO: Implement kitchen notification via Supabase Broadcast
    }
    
    public async Task NotifyTableAsync(Guid tableId, string message, CancellationToken cancellationToken = default)
    {
        // TODO: Implement table notification via Supabase Broadcast
    }
}
