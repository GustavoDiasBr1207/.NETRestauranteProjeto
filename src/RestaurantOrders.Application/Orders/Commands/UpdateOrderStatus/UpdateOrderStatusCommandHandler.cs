namespace RestaurantOrders.Application.Orders.Commands.UpdateOrderStatus;

using MediatR;
using RestaurantOrders.Domain.Interfaces.Repositories;
using RestaurantOrders.Domain.Enums;
using RestaurantOrders.Domain.Exceptions;

public class UpdateOrderStatusCommandHandler(IOrderRepository orderRepository)
    : IRequestHandler<UpdateOrderStatusCommand>
{
    public async Task Handle(UpdateOrderStatusCommand request, CancellationToken ct)
    {
        var order = await orderRepository.GetByIdWithItemsAsync(request.OrderId, ct)
            ?? throw new NotFoundException($"Pedido '{request.OrderId}' não encontrado.");

        switch (request.NewStatus)
        {
            case OrderStatusEnum.Confirmed:  order.Confirm();         break;
            case OrderStatusEnum.Preparing:  order.StartPreparing();  break;
            case OrderStatusEnum.Ready:      order.MarkReady();       break;
            case OrderStatusEnum.Delivered:  order.Deliver();         break;
            case OrderStatusEnum.Cancelled:  order.Cancel();          break;
            default:
                throw new InvalidOrderStatusException($"Transição para '{request.NewStatus}' não é suportada via este endpoint.");
        }

        await orderRepository.UpdateAsync(order, ct);
    }
}
