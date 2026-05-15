namespace RestaurantOrders.Domain.Interfaces.Repositories;

using RestaurantOrders.Domain.Entities;

/// <summary>Repositório de mesas — commit via <see cref="IUnitOfWork"/>.</summary>
public interface ITableRepository
{
    Task<Table?>       GetByIdAsync(Guid tableId, CancellationToken ct = default);
    Task<Table?>       GetByQrCodeTokenAsync(string qrCodeToken, CancellationToken ct = default);
    Task<List<Table>>  GetByRestaurantAsync(Guid restaurantId, CancellationToken ct = default);
    Task               AddAsync(Table table, CancellationToken ct = default);
    Task               UpdateAsync(Table table, CancellationToken ct = default);
}
