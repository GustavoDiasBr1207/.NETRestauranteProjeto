namespace RestaurantOrders.Application.Orders.Queries.GetOrdersByTable;

using MediatR;
using RestaurantOrders.Application.Common.DTOs;

/// <summary>Retorna todos os pedidos de uma mesa, ordenados do mais recente para o mais antigo.</summary>
public class GetOrdersByTableQuery : IRequest<List<OrderDto>>
{
    public Guid TableId { get; set; }
}
