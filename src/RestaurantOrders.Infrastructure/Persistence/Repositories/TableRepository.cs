namespace RestaurantOrders.Infrastructure.Persistence.Repositories;

using RestaurantOrders.Domain.Entities;
using RestaurantOrders.Domain.Interfaces.Repositories;

/// <summary>
/// Repository implementation for Restaurant entity
/// </summary>
public class RestaurantRepository : IRestaurantRepository
{
    private readonly ApplicationDbContext _context;
    
    public RestaurantRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<Restaurant?> GetByIdAsync(Guid restaurantId, CancellationToken cancellationToken = default)
    {
        // TODO: Implement GetByIdAsync
        return null;
    }
    
    public async Task<Restaurant?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default)
    {
        // TODO: Implement GetBySlugAsync
        return null;
    }
    
    public async Task<List<Restaurant>> GetAllActiveAsync(CancellationToken cancellationToken = default)
    {
        // TODO: Implement GetAllActiveAsync
        return new();
    }
    
    public async Task AddAsync(Restaurant restaurant, CancellationToken cancellationToken = default)
    {
        // TODO: Implement AddAsync
    }
    
    public async Task UpdateAsync(Restaurant restaurant, CancellationToken cancellationToken = default)
    {
        // TODO: Implement UpdateAsync
    }
}
