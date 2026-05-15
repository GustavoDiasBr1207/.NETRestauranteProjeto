namespace RestaurantOrders.Application.Common.DTOs;

/// <summary>Representação de um pedido retornada pela API.</summary>
public class OrderDto
{
    /// <summary>Identificador único do pedido.</summary>
    public Guid Id { get; set; }

    /// <summary>Restaurante ao qual o pedido pertence.</summary>
    public Guid RestaurantId { get; set; }

    /// <summary>Mesa associada ao pedido.</summary>
    public Guid TableId { get; set; }

    /// <summary>
    /// Status atual do pedido.
    /// Valores possíveis: <c>Draft</c>, <c>Pending</c>, <c>Confirmed</c>,
    /// <c>Preparing</c>, <c>Ready</c>, <c>Delivered</c>, <c>Cancelled</c>.
    /// </summary>
    public string Status { get; set; } = string.Empty;

    /// <summary>Valor total do pedido em centavos (BRL).</summary>
    public decimal TotalAmount { get; set; }

    /// <summary>Moeda do valor total (sempre <c>BRL</c>).</summary>
    public string Currency { get; set; } = "BRL";

    /// <summary>Observações gerais do pedido (opcional).</summary>
    public string? Notes { get; set; }

    /// <summary>Data/hora UTC em que o pedido foi submetido à cozinha.</summary>
    public DateTime? PlacedAt { get; set; }

    /// <summary>Data/hora UTC de criação do pedido (rascunho).</summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>Itens que compõem o pedido.</summary>
    public List<OrderItemDto> Items { get; set; } = [];
}
