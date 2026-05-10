namespace RestaurantOrders.Domain.Interfaces.Services;

/// <summary>
/// Service interface for notifying kitchen about orders
/// </summary>
public interface INotificationService
{
    Task NotifyKitchenAsync(Guid restaurantId, Guid orderId, CancellationToken cancellationToken = default);
    Task NotifyTableAsync(Guid tableId, string message, CancellationToken cancellationToken = default);
}
