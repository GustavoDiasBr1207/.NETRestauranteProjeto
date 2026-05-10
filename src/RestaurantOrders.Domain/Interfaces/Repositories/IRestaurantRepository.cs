namespace RestaurantOrders.Domain.Interfaces.Repositories;

using RestaurantOrders.Domain.Entities;

/// <summary>
/// Repository interface for Restaurant entity
/// </summary>
public interface IRestaurantRepository
{
    Task<Restaurant?> GetByIdAsync(Guid restaurantId, CancellationToken cancellationToken = default);
    Task<Restaurant?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default);
    Task<List<Restaurant>> GetAllActiveAsync(CancellationToken cancellationToken = default);
    Task AddAsync(Restaurant restaurant, CancellationToken cancellationToken = default);
    Task UpdateAsync(Restaurant restaurant, CancellationToken cancellationToken = default);
}
