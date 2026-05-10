namespace RestaurantOrders.Application.Orders.Queries.GetOrderById;

using MediatR;
using RestaurantOrders.Application.Common.DTOs;

/// <summary>
/// Query to get an order by id
/// </summary>
public class GetOrderByIdQuery : IRequest<OrderDto?>
{
    public Guid OrderId { get; set; }
}
