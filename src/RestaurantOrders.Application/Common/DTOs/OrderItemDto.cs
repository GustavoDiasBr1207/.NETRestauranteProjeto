namespace RestaurantOrders.Application.Common.DTOs;

/// <summary>
/// DTO for OrderItem
/// </summary>
public class OrderItemDto
{
    public Guid Id { get; set; }
    public string MenuItemName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal Subtotal { get; set; }
}
