namespace RestaurantOrders.Domain.Entities;

using RestaurantOrders.Domain.Common;
using RestaurantOrders.Domain.ValueObjects;

/// <summary>
/// Menu item entity
/// </summary>
public class MenuItem : BaseEntity
{
    public Guid CategoryId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public Money Price { get; set; } = null!;
    public string? ImageUrl { get; set; }
    public bool IsAvailable { get; set; }
    
    public static MenuItem Create(Guid categoryId, string name, string description, Money price, string? imageUrl = null)
    {
        return new MenuItem
        {
            CategoryId = categoryId,
            Name = name ?? throw new ArgumentNullException(nameof(name)),
            Description = description ?? string.Empty,
            Price = price ?? throw new ArgumentNullException(nameof(price)),
            ImageUrl = imageUrl,
            IsAvailable = true
        };
    }
}
