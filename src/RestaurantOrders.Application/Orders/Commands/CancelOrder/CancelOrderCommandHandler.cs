namespace RestaurantOrders.Application.Orders.Commands.CancelOrder;

using MediatR;
using RestaurantOrders.Domain.Exceptions;
using RestaurantOrders.Domain.Interfaces.Repositories;

public class CancelOrderCommandHandler(IOrderRepository orderRepository)
    : IRequestHandler<CancelOrderCommand>
{
    public async Task Handle(CancelOrderCommand request, CancellationToken ct)
    {
        var order = await orderRepository.GetByIdWithItemsAsync(request.OrderId, ct)
            ?? throw new NotFoundException($"Pedido '{request.OrderId}' não encontrado.");

        order.Cancel();
        await orderRepository.UpdateAsync(order, ct);
    }
}
