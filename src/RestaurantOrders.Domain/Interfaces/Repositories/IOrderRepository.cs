namespace RestaurantOrders.Domain.Interfaces.Repositories;

using RestaurantOrders.Domain.Entities;

/// <summary>
/// Repository interface for Order entity
/// </summary>
public interface IOrderRepository
{
    Task<Order?> GetByIdAsync(Guid orderId, CancellationToken cancellationToken = default);
    Task<Order?> GetByIdWithItemsAsync(Guid orderId, CancellationToken cancellationToken = default);
    Task<List<Order>> GetActiveByRestaurantAsync(Guid restaurantId, CancellationToken cancellationToken = default);
    Task<List<Order>> GetByTableIdAsync(Guid tableId, CancellationToken cancellationToken = default);
    Task AddAsync(Order order, CancellationToken cancellationToken = default);
    Task UpdateAsync(Order order, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid orderId, CancellationToken cancellationToken = default);
}
