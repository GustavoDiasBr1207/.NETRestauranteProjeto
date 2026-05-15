namespace RestaurantOrders.Infrastructure.Persistence.Repositories;

using Microsoft.EntityFrameworkCore;
using RestaurantOrders.Domain.Entities;
using RestaurantOrders.Domain.Enums;
using RestaurantOrders.Domain.Interfaces.Repositories;

public class OrderRepository(ApplicationDbContext context) : IOrderRepository
{
    public async Task<Order?> GetByIdAsync(Guid orderId, CancellationToken ct = default)
        => await context.Orders.FindAsync([orderId], ct);

    public async Task<Order?> GetByIdWithItemsAsync(Guid orderId, CancellationToken ct = default)
        => await context.Orders
            .Include(o => o.Items)
            .FirstOrDefaultAsync(o => o.Id == orderId, ct);

    public async Task<List<Order>> GetActiveByRestaurantAsync(Guid restaurantId, CancellationToken ct = default)
        => await context.Orders
            .Include(o => o.Items)
            .Where(o => o.RestaurantId == restaurantId
                     && o.Status != OrderStatusEnum.Delivered
                     && o.Status != OrderStatusEnum.Cancelled)
            .OrderByDescending(o => o.PlacedAt)
            .ToListAsync(ct);

    public async Task<List<Order>> GetByTableIdAsync(Guid tableId, CancellationToken ct = default)
        => await context.Orders
            .Include(o => o.Items)
            .Where(o => o.TableId == tableId)
            .OrderByDescending(o => o.CreatedAt)
            .ToListAsync(ct);

    public async Task AddAsync(Order order, CancellationToken ct = default)
    {
        await context.Orders.AddAsync(order, ct);
        await context.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(Order order, CancellationToken ct = default)
    {
        context.Orders.Update(order);
        await context.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(Guid orderId, CancellationToken ct = default)
    {
        var order = await context.Orders.FindAsync([orderId], ct);
        if (order is not null)
        {
            context.Orders.Remove(order);
            await context.SaveChangesAsync(ct);
        }
    }
}
