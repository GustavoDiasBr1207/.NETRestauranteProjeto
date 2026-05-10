namespace RestaurantOrders.Application.Orders.Queries.GetOrdersByTable;

using MediatR;
using AutoMapper;
using RestaurantOrders.Application.Common.DTOs;
using RestaurantOrders.Domain.Interfaces.Repositories;

/// <summary>
/// Handler for GetOrdersByTableQuery
/// </summary>
public class GetOrdersByTableQueryHandler : IRequestHandler<GetOrdersByTableQuery, List<OrderDto>>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IMapper _mapper;
    
    public GetOrdersByTableQueryHandler(IOrderRepository orderRepository, IMapper mapper)
    {
        _orderRepository = orderRepository;
        _mapper = mapper;
    }
    
    public async Task<List<OrderDto>> Handle(GetOrdersByTableQuery request, CancellationToken cancellationToken)
    {
        // TODO: Implement logic to get orders by table
        var orders = await _orderRepository.GetByTableIdAsync(request.TableId, cancellationToken);
        return _mapper.Map<List<OrderDto>>(orders);
    }
}
