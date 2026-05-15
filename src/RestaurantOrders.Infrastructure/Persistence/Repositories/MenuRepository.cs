namespace RestaurantOrders.Infrastructure.Persistence.Repositories;

using Microsoft.EntityFrameworkCore;
using RestaurantOrders.Domain.Entities;
using RestaurantOrders.Domain.Interfaces.Repositories;

public class MenuRepository(ApplicationDbContext context) : IMenuRepository
{
    public async Task<List<MenuCategory>> GetCategoriesWithItemsAsync(Guid restaurantId, CancellationToken ct = default)
        => await context.MenuCategories
            .Include(c => c.Items)
            .Where(c => c.RestaurantId == restaurantId && c.IsActive)
            .OrderBy(c => c.DisplayOrder)
            .ToListAsync(ct);

    public async Task<MenuItem?> GetItemByIdAsync(Guid menuItemId, CancellationToken ct = default)
        => await context.MenuItems.FindAsync([menuItemId], ct);

    public async Task<List<MenuItem>> GetItemsByRestaurantAsync(Guid restaurantId, CancellationToken ct = default)
        => await context.MenuItems
            .Where(i => i.RestaurantId == restaurantId)
            .OrderBy(i => i.DisplayOrder)
            .ToListAsync(ct);

    // Métodos de escrita apenas rastreiam mudanças — commit é feito pelo TransactionBehavior via IUnitOfWork

    public async Task AddCategoryAsync(MenuCategory category, CancellationToken ct = default)
        => await context.MenuCategories.AddAsync(category, ct);

    public async Task AddMenuItemAsync(MenuItem menuItem, CancellationToken ct = default)
        => await context.MenuItems.AddAsync(menuItem, ct);

    public Task UpdateMenuItemAsync(MenuItem menuItem, CancellationToken ct = default)
    {
        context.MenuItems.Update(menuItem);
        return Task.CompletedTask;
    }
}
