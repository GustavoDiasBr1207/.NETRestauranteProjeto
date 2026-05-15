namespace RestaurantOrders.Application.Orders.Commands.CreateOrder;

using MediatR;
using RestaurantOrders.Domain.Entities;
using RestaurantOrders.Domain.Exceptions;
using RestaurantOrders.Domain.Interfaces.Repositories;

public class CreateOrderCommandHandler(
    IOrderRepository      orderRepository,
    IRestaurantRepository restaurantRepository,
    ITableRepository      tableRepository)
    : IRequestHandler<CreateOrderCommand, Guid>
{
    public async Task<Guid> Handle(CreateOrderCommand request, CancellationToken ct)
    {
        // Valida existência das FKs antes de criar o pedido
        _ = await restaurantRepository.GetByIdAsync(request.RestaurantId, ct)
            ?? throw new NotFoundException($"Restaurante '{request.RestaurantId}' não encontrado.");

        _ = await tableRepository.GetByIdAsync(request.TableId, ct)
            ?? throw new NotFoundException($"Mesa '{request.TableId}' não encontrada.");

        var order = Order.Create(request.RestaurantId, request.TableId, request.CustomerId);
        await orderRepository.AddAsync(order, ct);

        return order.Id;
    }
}
