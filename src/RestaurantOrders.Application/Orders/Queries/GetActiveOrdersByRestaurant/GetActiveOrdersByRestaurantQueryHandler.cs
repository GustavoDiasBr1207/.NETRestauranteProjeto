namespace RestaurantOrders.Application.Orders.Queries.GetActiveOrdersByRestaurant;

using MediatR;
using AutoMapper;
using RestaurantOrders.Application.Common.DTOs;
using RestaurantOrders.Domain.Interfaces.Repositories;

/// <summary>
/// Handler for GetActiveOrdersByRestaurantQuery
/// </summary>
public class GetActiveOrdersByRestaurantQueryHandler : IRequestHandler<GetActiveOrdersByRestaurantQuery, List<OrderDto>>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IMapper _mapper;
    
    public GetActiveOrdersByRestaurantQueryHandler(IOrderRepository orderRepository, IMapper mapper)
    {
        _orderRepository = orderRepository;
        _mapper = mapper;
    }
    
    public async Task<List<OrderDto>> Handle(GetActiveOrdersByRestaurantQuery request, CancellationToken cancellationToken)
    {
        // TODO: Implement logic to get active orders by restaurant
        var orders = await _orderRepository.GetActiveByRestaurantAsync(request.RestaurantId, cancellationToken);
        return _mapper.Map<List<OrderDto>>(orders);
    }
}
