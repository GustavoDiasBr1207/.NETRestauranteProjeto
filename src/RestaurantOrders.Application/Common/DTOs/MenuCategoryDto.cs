namespace RestaurantOrders.Application.Common.DTOs;

/// <summary>
/// DTO for MenuCategory
/// </summary>
public class MenuCategoryDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public List<MenuItemDto> Items { get; set; } = new();
}
