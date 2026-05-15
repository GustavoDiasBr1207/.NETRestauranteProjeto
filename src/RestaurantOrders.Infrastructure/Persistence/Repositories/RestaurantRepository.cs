namespace RestaurantOrders.Infrastructure.Persistence.Repositories;

using Microsoft.EntityFrameworkCore;
using RestaurantOrders.Domain.Entities;
using RestaurantOrders.Domain.Interfaces.Repositories;

public class RestaurantRepository(ApplicationDbContext context) : IRestaurantRepository
{
    public async Task<Restaurant?> GetByIdAsync(Guid restaurantId, CancellationToken ct = default)
        => await context.Restaurants.FindAsync([restaurantId], ct);

    public async Task<Restaurant?> GetBySlugAsync(string slug, CancellationToken ct = default)
        => await context.Restaurants
            .FirstOrDefaultAsync(r => r.Slug == slug && r.IsActive, ct);

    public async Task<List<Restaurant>> GetAllActiveAsync(CancellationToken ct = default)
        => await context.Restaurants
            .Where(r => r.IsActive)
            .OrderBy(r => r.Name)
            .ToListAsync(ct);

    public async Task AddAsync(Restaurant restaurant, CancellationToken ct = default)
    {
        await context.Restaurants.AddAsync(restaurant, ct);
        await context.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(Restaurant restaurant, CancellationToken ct = default)
    {
        context.Restaurants.Update(restaurant);
        await context.SaveChangesAsync(ct);
    }
}
