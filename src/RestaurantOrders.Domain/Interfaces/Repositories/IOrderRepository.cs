namespace RestaurantOrders.Domain.Interfaces.Repositories;

using RestaurantOrders.Domain.Entities;

/// <summary>Repositório de pedidos — apenas leitura e rastreamento; commit via <see cref="IUnitOfWork"/>.</summary>
public interface IOrderRepository
{
    Task<Order?>       GetByIdAsync(Guid orderId, CancellationToken ct = default);
    Task<Order?>       GetByIdWithItemsAsync(Guid orderId, CancellationToken ct = default);
    Task<List<Order>>  GetActiveByRestaurantAsync(Guid restaurantId, CancellationToken ct = default);
    Task<List<Order>>  GetByTableIdAsync(Guid tableId, CancellationToken ct = default);
    Task               AddAsync(Order order, CancellationToken ct = default);
    Task               UpdateAsync(Order order, CancellationToken ct = default);
    Task               DeleteAsync(Guid orderId, CancellationToken ct = default);
}
