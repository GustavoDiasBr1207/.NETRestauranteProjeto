namespace RestaurantOrders.Application.Orders.Commands.SubmitOrder;

using MediatR;
using RestaurantOrders.Domain.Exceptions;
using RestaurantOrders.Domain.Interfaces.Repositories;

public class SubmitOrderCommandHandler(IOrderRepository orderRepository)
    : IRequestHandler<SubmitOrderCommand>
{
    public async Task Handle(SubmitOrderCommand request, CancellationToken ct)
    {
        var order = await orderRepository.GetByIdWithItemsAsync(request.OrderId, ct)
            ?? throw new NotFoundException($"Pedido '{request.OrderId}' não encontrado.");

        // order.Submit() publica OrderPlacedEvent — a notificação à cozinha é
        // tratada pelo OrderPlacedEventHandler, despachado pelo SaveChangesAsync.
        order.Submit();
        await orderRepository.UpdateAsync(order, ct);
    }
}
