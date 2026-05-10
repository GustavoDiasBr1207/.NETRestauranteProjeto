namespace RestaurantOrders.Application.Orders.Commands.CreateOrder;

using MediatR;
using RestaurantOrders.Domain.Entities;
using RestaurantOrders.Domain.Interfaces.Repositories;

/// <summary>
/// Handler for CreateOrderCommand
/// </summary>
public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, Guid>
{
    private readonly IOrderRepository _orderRepository;
    
    public CreateOrderCommandHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }
    
    public async Task<Guid> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        // TODO: Implement logic to create order
        var order = Order.Create(request.RestaurantId, request.TableId, request.CustomerId);
        await _orderRepository.AddAsync(order, cancellationToken);
        
        return order.Id;
    }
}
