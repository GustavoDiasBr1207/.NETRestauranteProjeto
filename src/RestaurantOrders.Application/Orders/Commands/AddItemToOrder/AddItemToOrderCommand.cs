namespace RestaurantOrders.Application.Orders.Commands.AddItemToOrder;

using System.Text.Json.Serialization;
using MediatR;

/// <summary>Adiciona um item ao carrinho do pedido.</summary>
public class AddItemToOrderCommand : IRequest
{
    /// <summary>ID do pedido (preenchido via rota — não enviar no body).</summary>
    [JsonIgnore]
    public Guid OrderId { get; set; }

    /// <summary>ID do item do cardápio a adicionar.</summary>
    public Guid MenuItemId { get; set; }

    /// <summary>Quantidade desejada (mínimo 1).</summary>
    public int Quantity { get; set; }

    /// <summary>Observações para o item (ex: "sem cebola", "bem passado").</summary>
    public string? Notes { get; set; }
}
