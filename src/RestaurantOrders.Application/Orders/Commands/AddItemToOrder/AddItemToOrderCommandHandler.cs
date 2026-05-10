namespace RestaurantOrders.Application.Orders.Commands.AddItemToOrder;

using MediatR;
using RestaurantOrders.Domain.Interfaces.Repositories;
using RestaurantOrders.Domain.Exceptions;

/// <summary>
/// Handler for AddItemToOrderCommand
/// </summary>
public class AddItemToOrderCommandHandler : IRequestHandler<AddItemToOrderCommand>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IMenuRepository _menuRepository;
    
    public AddItemToOrderCommandHandler(IOrderRepository orderRepository, IMenuRepository menuRepository)
    {
        _orderRepository = orderRepository;
        _menuRepository = menuRepository;
    }
    
    public async Task Handle(AddItemToOrderCommand request, CancellationToken cancellationToken)
    {
        // TODO: Implement logic to add item to order
        var order = await _orderRepository.GetByIdWithItemsAsync(request.OrderId, cancellationToken)
            ?? throw new NotFoundException($"Order with id '{request.OrderId}' not found");
        
        var menuItem = await _menuRepository.GetItemByIdAsync(request.MenuItemId, cancellationToken)
            ?? throw new NotFoundException($"Menu item with id '{request.MenuItemId}' not found");
        
        order.AddItem(menuItem, request.Quantity, request.Notes);
        await _orderRepository.UpdateAsync(order, cancellationToken);
    }
}
