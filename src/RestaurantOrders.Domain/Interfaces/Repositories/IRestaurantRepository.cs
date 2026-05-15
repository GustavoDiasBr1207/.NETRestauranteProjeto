namespace RestaurantOrders.Domain.Interfaces.Repositories;

using RestaurantOrders.Domain.Entities;

/// <summary>Repositório de restaurantes — commit via <see cref="IUnitOfWork"/>.</summary>
public interface IRestaurantRepository
{
    Task<Restaurant?>       GetByIdAsync(Guid restaurantId, CancellationToken ct = default);
    Task<Restaurant?>       GetBySlugAsync(string slug, CancellationToken ct = default);
    Task<List<Restaurant>>  GetAllActiveAsync(CancellationToken ct = default);
    Task                    AddAsync(Restaurant restaurant, CancellationToken ct = default);
    Task                    UpdateAsync(Restaurant restaurant, CancellationToken ct = default);
}
