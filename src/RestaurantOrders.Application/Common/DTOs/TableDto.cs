namespace RestaurantOrders.Application.Common.DTOs;

/// <summary>
/// DTO for Table
/// </summary>
public class TableDto
{
    public Guid Id { get; set; }
    public Guid RestaurantId { get; set; }
    public int Number { get; set; }
    public string QrCodeToken { get; set; } = string.Empty;
}
