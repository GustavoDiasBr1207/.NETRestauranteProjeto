namespace RestaurantOrders.Application.Common.DTOs;

/// <summary>
/// DTO for Order
/// </summary>
public class OrderDto
{
    public Guid Id { get; set; }
    public int TableNumber { get; set; }
    public string Status { get; set; } = string.Empty;
    public List<OrderItemDto> Items { get; set; } = new();
    public decimal TotalAmount { get; set; }
    public DateTime PlacedAt { get; set; }
}
