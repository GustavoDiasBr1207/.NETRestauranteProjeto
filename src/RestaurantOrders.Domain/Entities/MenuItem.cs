using RestaurantOrders.Domain.Common;
using RestaurantOrders.Domain.ValueObjects;

namespace RestaurantOrders.Domain.Entities;

public class MenuItem : BaseEntity
{
    public Guid   RestaurantId { get; private set; }
    public Guid   CategoryId   { get; private set; }
    public string Name         { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public Money  Price        { get; private set; } = null!;
    public string? ImageUrl    { get; private set; }
    public bool   IsAvailable  { get; private set; } = true;
    public int    DisplayOrder { get; private set; }

    // Navigation
    public MenuCategory? Category    { get; private set; }
    public Restaurant?   Restaurant  { get; private set; }

    private MenuItem() { } // EF Core

    public static MenuItem Create(
        Guid   restaurantId,
        Guid   categoryId,
        string name,
        Money  price,
        string? description  = null,
        string? imageUrl     = null,
        int     displayOrder = 0)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        return new MenuItem
        {
            RestaurantId = restaurantId,
            CategoryId   = categoryId,
            Name         = name.Trim(),
            Price        = price,
            Description  = description?.Trim(),
            ImageUrl     = imageUrl,
            DisplayOrder = displayOrder
        };
    }

    public void Update(string name, Money price, string? description, string? imageUrl, int displayOrder)
    {
        Name         = name.Trim();
        Price        = price;
        Description  = description?.Trim();
        ImageUrl     = imageUrl;
        DisplayOrder = displayOrder;
    }

    public void MakeAvailable()   => IsAvailable = true;
    public void MakeUnavailable() => IsAvailable = false;
}