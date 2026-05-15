namespace RestaurantOrders.Infrastructure.Persistence.Repositories;

using Microsoft.EntityFrameworkCore;
using RestaurantOrders.Domain.Entities;
using RestaurantOrders.Domain.Interfaces.Repositories;

public class TableRepository(ApplicationDbContext context) : ITableRepository
{
    public async Task<Table?> GetByIdAsync(Guid tableId, CancellationToken ct = default)
        => await context.Tables.FindAsync([tableId], ct);

    public async Task<Table?> GetByQrCodeTokenAsync(string qrCodeToken, CancellationToken ct = default)
        => await context.Tables
            .FirstOrDefaultAsync(t => t.QrCode.Token == qrCodeToken && t.IsActive, ct);

    public async Task<List<Table>> GetByRestaurantAsync(Guid restaurantId, CancellationToken ct = default)
        => await context.Tables
            .Where(t => t.RestaurantId == restaurantId)
            .OrderBy(t => t.Number)
            .ToListAsync(ct);

    public async Task AddAsync(Table table, CancellationToken ct = default)
    {
        await context.Tables.AddAsync(table, ct);
        await context.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(Table table, CancellationToken ct = default)
    {
        context.Tables.Update(table);
        await context.SaveChangesAsync(ct);
    }
}
