namespace RestaurantOrders.Application.Common.DTOs;

/// <summary>Item de um pedido retornado pela API.</summary>
public class OrderItemDto
{
    /// <summary>Identificador único do item no pedido.</summary>
    public Guid Id { get; set; }

    /// <summary>ID do item no cardápio.</summary>
    public Guid MenuItemId { get; set; }

    /// <summary>Nome do item no cardápio no momento do pedido.</summary>
    public string MenuItemName { get; set; } = string.Empty;

    /// <summary>Quantidade solicitada.</summary>
    public int Quantity { get; set; }

    /// <summary>Preço unitário do item no momento do pedido (BRL).</summary>
    public decimal UnitPrice { get; set; }

    /// <summary>Subtotal do item (<c>UnitPrice × Quantity</c>, BRL).</summary>
    public decimal Subtotal { get; set; }

    /// <summary>Observações do cliente para este item (ex: "sem cebola").</summary>
    public string? Notes { get; set; }
}
