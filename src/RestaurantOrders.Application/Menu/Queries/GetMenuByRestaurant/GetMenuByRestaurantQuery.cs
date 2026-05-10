namespace RestaurantOrders.Application.Menu.Queries.GetMenuByRestaurant;

using MediatR;
using RestaurantOrders.Application.Common.DTOs;

/// <summary>
/// Query to get menu by restaurant
/// </summary>
public class GetMenuByRestaurantQuery : IRequest<List<MenuCategoryDto>>
{
    public Guid RestaurantId { get; set; }
    public Guid? CategoryId { get; set; }
}
