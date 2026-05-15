namespace RestaurantOrders.Application.Orders.Commands.RemoveItemFromOrder;

using MediatR;
using RestaurantOrders.Domain.Exceptions;
using RestaurantOrders.Domain.Interfaces.Repositories;

public class RemoveItemFromOrderCommandHandler(IOrderRepository orderRepository)
    : IRequestHandler<RemoveItemFromOrderCommand>
{
    public async Task Handle(RemoveItemFromOrderCommand request, CancellationToken ct)
    {
        var order = await orderRepository.GetByIdWithItemsAsync(request.OrderId, ct)
            ?? throw new NotFoundException($"Pedido '{request.OrderId}' não encontrado.");

        order.RemoveItem(request.OrderItemId);
        await orderRepository.UpdateAsync(order, ct);
    }
}
