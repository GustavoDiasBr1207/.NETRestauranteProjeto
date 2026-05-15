namespace RestaurantOrders.Application.Common.DTOs;

/// <summary>Item do cardápio retornado pela API.</summary>
public class MenuItemDto
{
    /// <summary>Identificador único do item.</summary>
    public Guid Id { get; set; }

    /// <summary>Nome do item (ex: "Risoto de Funghi").</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>Descrição detalhada do item.</summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>Preço unitário em BRL.</summary>
    public decimal Price { get; set; }

    /// <summary>URL da imagem do item (opcional).</summary>
    public string? ImageUrl { get; set; }

    /// <summary>Indica se o item está disponível para pedido.</summary>
    public bool IsAvailable { get; set; }
}
