namespace RestaurantOrders.Application.Menu.Queries.GetMenuItemById;

using MediatR;
using RestaurantOrders.Application.Common.DTOs;

/// <summary>
/// Query to get menu item by id
/// </summary>
public class GetMenuItemByIdQuery : IRequest<MenuItemDto?>
{
    public Guid MenuItemId { get; set; }
}
