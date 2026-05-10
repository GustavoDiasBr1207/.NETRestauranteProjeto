namespace RestaurantOrders.Application.Common.DTOs;

/// <summary>
/// DTO for MenuItem
/// </summary>
public class MenuItemDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string? ImageUrl { get; set; }
    public bool IsAvailable { get; set; }
}
