namespace RestaurantOrders.Application.Orders.Commands.CreateOrder;

using MediatR;

/// <summary>Cria um novo pedido em modo rascunho (carrinho).</summary>
public class CreateOrderCommand : IRequest<Guid>
{
    /// <summary>ID do restaurante onde o pedido será feito.</summary>
    public Guid RestaurantId { get; set; }

    /// <summary>ID da mesa associada ao pedido.</summary>
    public Guid TableId { get; set; }

    /// <summary>ID do cliente (opcional — permite rastrear pedidos por cliente).</summary>
    public Guid? CustomerId { get; set; }
}
