namespace RestaurantOrders.Application.Orders.Commands.UpdateOrderStatus;

using System.Text.Json.Serialization;
using MediatR;
using RestaurantOrders.Domain.Enums;

/// <summary>Avança o status do pedido (uso da cozinha / gerente).</summary>
public class UpdateOrderStatusCommand : IRequest
{
    /// <summary>ID do pedido (preenchido via rota — não enviar no body).</summary>
    [JsonIgnore]
    public Guid OrderId { get; set; }

    /// <summary>
    /// Novo status desejado.
    /// Transições válidas: <c>Pending→Confirmed</c>, <c>Confirmed→Preparing</c>,
    /// <c>Preparing→Ready</c>, <c>Ready→Delivered</c>, qualquer→<c>Cancelled</c>.
    /// </summary>
    public OrderStatusEnum NewStatus { get; set; }
}
