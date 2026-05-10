namespace RestaurantOrders.Infrastructure.Persistence.Repositories;

using RestaurantOrders.Domain.Entities;
using RestaurantOrders.Domain.Interfaces.Repositories;

/// <summary>
/// Repository implementation for Menu entities (Category and MenuItem)
/// </summary>
public class MenuRepository : IMenuRepository
{
    private readonly ApplicationDbContext _context;
    
    public MenuRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<List<MenuCategory>> GetCategoriesWithItemsAsync(Guid restaurantId, CancellationToken cancellationToken = default)
    {
        // TODO: Implement GetCategoriesWithItemsAsync with Include for MenuItems
        return new();
    }
    
    public async Task<MenuItem?> GetItemByIdAsync(Guid menuItemId, CancellationToken cancellationToken = default)
    {
        // TODO: Implement GetItemByIdAsync
        return null;
    }
    
    public async Task<List<MenuItem>> GetItemsByRestaurantAsync(Guid restaurantId, CancellationToken cancellationToken = default)
    {
        // TODO: Implement GetItemsByRestaurantAsync
        return new();
    }
    
    public async Task AddCategoryAsync(MenuCategory category, CancellationToken cancellationToken = default)
    {
        // TODO: Implement AddCategoryAsync
    }
    
    public async Task AddMenuItemAsync(MenuItem menuItem, CancellationToken cancellationToken = default)
    {
        // TODO: Implement AddMenuItemAsync
    }
    
    public async Task UpdateMenuItemAsync(MenuItem menuItem, CancellationToken cancellationToken = default)
    {
        // TODO: Implement UpdateMenuItemAsync
    }
}
