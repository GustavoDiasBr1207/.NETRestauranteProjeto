namespace RestaurantOrders.Application.Orders.Commands.AddItemToOrder;

using MediatR;
using RestaurantOrders.Domain.Exceptions;
using RestaurantOrders.Domain.Interfaces.Repositories;

public class AddItemToOrderCommandHandler(IOrderRepository orderRepository, IMenuRepository menuRepository)
    : IRequestHandler<AddItemToOrderCommand>
{
    public async Task Handle(AddItemToOrderCommand request, CancellationToken ct)
    {
        var order = await orderRepository.GetByIdWithItemsAsync(request.OrderId, ct)
            ?? throw new NotFoundException($"Pedido '{request.OrderId}' não encontrado.");

        var menuItem = await menuRepository.GetItemByIdAsync(request.MenuItemId, ct)
            ?? throw new NotFoundException($"Item de cardápio '{request.MenuItemId}' não encontrado.");

        order.AddItem(menuItem, request.Quantity, request.Notes);
        await orderRepository.UpdateAsync(order, ct);
    }
}
