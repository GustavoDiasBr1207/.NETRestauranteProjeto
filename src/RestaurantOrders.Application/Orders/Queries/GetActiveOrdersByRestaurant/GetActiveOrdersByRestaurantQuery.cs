namespace RestaurantOrders.Application.Orders.Queries.GetActiveOrdersByRestaurant;

using MediatR;
using RestaurantOrders.Application.Common.DTOs;

/// <summary>
/// Query to get active orders by restaurant
/// </summary>
public class GetActiveOrdersByRestaurantQuery : IRequest<List<OrderDto>>
{
    public Guid RestaurantId { get; set; }
}
