namespace RestaurantOrders.Application.Orders.Queries.GetOrderById;

using MediatR;
using AutoMapper;
using RestaurantOrders.Application.Common.DTOs;
using RestaurantOrders.Domain.Interfaces.Repositories;

/// <summary>
/// Handler for GetOrderByIdQuery
/// </summary>
public class GetOrderByIdQueryHandler : IRequestHandler<GetOrderByIdQuery, OrderDto?>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IMapper _mapper;
    
    public GetOrderByIdQueryHandler(IOrderRepository orderRepository, IMapper mapper)
    {
        _orderRepository = orderRepository;
        _mapper = mapper;
    }
    
    public async Task<OrderDto?> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
    {
        // TODO: Implement logic to get order by id
        var order = await _orderRepository.GetByIdWithItemsAsync(request.OrderId, cancellationToken);
        
        if (order == null)
            return null;
        
        return _mapper.Map<OrderDto>(order);
    }
}
