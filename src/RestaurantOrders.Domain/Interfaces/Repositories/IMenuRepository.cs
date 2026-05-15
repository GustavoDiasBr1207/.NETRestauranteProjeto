namespace RestaurantOrders.Domain.Interfaces.Repositories;

using RestaurantOrders.Domain.Entities;

/// <summary>Repositório de cardápio (categorias e itens) — commit via <see cref="IUnitOfWork"/>.</summary>
public interface IMenuRepository
{
    Task<List<MenuCategory>> GetCategoriesWithItemsAsync(Guid restaurantId, CancellationToken ct = default);
    Task<MenuItem?>          GetItemByIdAsync(Guid menuItemId, CancellationToken ct = default);
    Task<List<MenuItem>>     GetItemsByRestaurantAsync(Guid restaurantId, CancellationToken ct = default);
    Task                     AddCategoryAsync(MenuCategory category, CancellationToken ct = default);
    Task                     AddMenuItemAsync(MenuItem menuItem, CancellationToken ct = default);
    Task                     UpdateMenuItemAsync(MenuItem menuItem, CancellationToken ct = default);
}
