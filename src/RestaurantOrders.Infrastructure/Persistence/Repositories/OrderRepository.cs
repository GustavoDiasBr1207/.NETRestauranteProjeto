namespace RestaurantOrders.Infrastructure.Persistence.Repositories;

using RestaurantOrders.Domain.Entities;
using RestaurantOrders.Domain.Interfaces.Repositories;

/// <summary>
/// Repository implementation for Order entity
/// </summary>
public class OrderRepository : IOrderRepository
{
    private readonly ApplicationDbContext _context;
    
    public OrderRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<Order?> GetByIdAsync(Guid orderId, CancellationToken cancellationToken = default)
    {
        // TODO: Implement GetByIdAsync
        return null;
    }
    
    public async Task<Order?> GetByIdWithItemsAsync(Guid orderId, CancellationToken cancellationToken = default)
    {
        // TODO: Implement GetByIdWithItemsAsync with Include for OrderItems
        return null;
    }
    
    public async Task<List<Order>> GetActiveByRestaurantAsync(Guid restaurantId, CancellationToken cancellationToken = default)
    {
        // TODO: Implement GetActiveByRestaurantAsync
        return new();
    }
    
    public async Task<List<Order>> GetByTableIdAsync(Guid tableId, CancellationToken cancellationToken = default)
    {
        // TODO: Implement GetByTableIdAsync
        return new();
    }
    
    public async Task AddAsync(Order order, CancellationToken cancellationToken = default)
    {
        // TODO: Implement AddAsync
    }
    
    public async Task UpdateAsync(Order order, CancellationToken cancellationToken = default)
    {
        // TODO: Implement UpdateAsync
    }
    
    public async Task DeleteAsync(Guid orderId, CancellationToken cancellationToken = default)
    {
        // TODO: Implement DeleteAsync
    }
}
