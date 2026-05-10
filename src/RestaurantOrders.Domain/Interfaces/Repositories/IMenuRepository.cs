namespace RestaurantOrders.Domain.Interfaces.Repositories;

using RestaurantOrders.Domain.Entities;

/// <summary>
/// Repository interface for Menu entities (Category and MenuItem)
/// </summary>
public interface IMenuRepository
{
    Task<List<MenuCategory>> GetCategoriesWithItemsAsync(Guid restaurantId, CancellationToken cancellationToken = default);
    Task<MenuItem?> GetItemByIdAsync(Guid menuItemId, CancellationToken cancellationToken = default);
    Task<List<MenuItem>> GetItemsByRestaurantAsync(Guid restaurantId, CancellationToken cancellationToken = default);
    Task AddCategoryAsync(MenuCategory category, CancellationToken cancellationToken = default);
    Task AddMenuItemAsync(MenuItem menuItem, CancellationToken cancellationToken = default);
    Task UpdateMenuItemAsync(MenuItem menuItem, CancellationToken cancellationToken = default);
}
