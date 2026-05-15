namespace RestaurantOrders.Application.Orders.Queries.GetOrdersByTable;

using AutoMapper;
using MediatR;
using RestaurantOrders.Application.Common.DTOs;
using RestaurantOrders.Domain.Interfaces.Repositories;

public class GetOrdersByTableQueryHandler(IOrderRepository orderRepository, IMapper mapper)
    : IRequestHandler<GetOrdersByTableQuery, List<OrderDto>>
{
    public async Task<List<OrderDto>> Handle(GetOrdersByTableQuery request, CancellationToken ct)
    {
        var orders = await orderRepository.GetByTableIdAsync(request.TableId, ct);
        return mapper.Map<List<OrderDto>>(orders);
    }
}
