namespace RestaurantOrders.Domain.Interfaces.Repositories;

using RestaurantOrders.Domain.Entities;

/// <summary>
/// Repository interface for Table entity
/// </summary>
public interface ITableRepository
{
    Task<Table?> GetByIdAsync(Guid tableId, CancellationToken cancellationToken = default);
    Task<Table?> GetByQrCodeTokenAsync(string qrCodeToken, CancellationToken cancellationToken = default);
    Task<List<Table>> GetByRestaurantAsync(Guid restaurantId, CancellationToken cancellationToken = default);
    Task AddAsync(Table table, CancellationToken cancellationToken = default);
    Task UpdateAsync(Table table, CancellationToken cancellationToken = default);
}
