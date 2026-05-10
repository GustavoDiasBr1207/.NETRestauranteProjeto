namespace RestaurantOrders.Infrastructure.Persistence.Repositories;

using RestaurantOrders.Domain.Entities;
using RestaurantOrders.Domain.Interfaces.Repositories;

/// <summary>
/// Repository implementation for Table entity
/// </summary>
public class TableRepository : ITableRepository
{
    private readonly ApplicationDbContext _context;
    
    public TableRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<Table?> GetByIdAsync(Guid tableId, CancellationToken cancellationToken = default)
    {
        // TODO: Implement GetByIdAsync
        return null;
    }
    
    public async Task<Table?> GetByQrCodeTokenAsync(string qrCodeToken, CancellationToken cancellationToken = default)
    {
        // TODO: Implement GetByQrCodeTokenAsync
        return null;
    }
    
    public async Task<List<Table>> GetByRestaurantAsync(Guid restaurantId, CancellationToken cancellationToken = default)
    {
        // TODO: Implement GetByRestaurantAsync
        return new();
    }
    
    public async Task AddAsync(Table table, CancellationToken cancellationToken = default)
    {
        // TODO: Implement AddAsync
    }
    
    public async Task UpdateAsync(Table table, CancellationToken cancellationToken = default)
    {
        // TODO: Implement UpdateAsync
    }
}
