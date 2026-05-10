namespace RestaurantOrders.Application.Orders.Queries.GetOrdersByTable;

using MediatR;
using RestaurantOrders.Application.Common.DTOs;

/// <summary>
/// Query to get orders by table
/// </summary>
public class GetOrdersByTableQuery : IRequest<List<OrderDto>>
{
    public Guid TableId { get; set; }
    public string? Status { get; set; }
}
