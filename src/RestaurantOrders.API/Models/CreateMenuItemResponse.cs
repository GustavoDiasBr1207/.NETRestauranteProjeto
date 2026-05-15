namespace RestaurantOrders.API.Models;

/// <summary>
/// Resposta ao criar um novo item no cardápio.
/// </summary>
/// <param name="MenuItemId">ID do item de cardápio recém-criado.</param>
public sealed record CreateMenuItemResponse(Guid MenuItemId);
