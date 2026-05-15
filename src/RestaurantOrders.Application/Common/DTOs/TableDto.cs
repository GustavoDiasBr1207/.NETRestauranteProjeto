namespace RestaurantOrders.Application.Common.DTOs;

/// <summary>Dados da mesa resolvida a partir do QR Code.</summary>
public class TableDto
{
    /// <summary>Identificador único da mesa.</summary>
    public Guid Id { get; set; }

    /// <summary>Restaurante ao qual a mesa pertence.</summary>
    public Guid RestaurantId { get; set; }

    /// <summary>Número da mesa exibido ao cliente.</summary>
    public int Number { get; set; }

    /// <summary>Token único do QR Code da mesa (32 caracteres hex).</summary>
    public string QrCodeToken { get; set; } = string.Empty;
}
