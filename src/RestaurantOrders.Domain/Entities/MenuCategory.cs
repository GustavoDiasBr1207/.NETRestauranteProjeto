using RestaurantOrders.Domain.Common;

namespace RestaurantOrders.Domain.Entities;

public class MenuCategory : BaseEntity
{
    public Guid   RestaurantId  { get; private set; }
    public string Name          { get; private set; } = string.Empty;
    public int    DisplayOrder  { get; private set; }
    public bool   IsActive      { get; private set; } = true;

    private readonly List<MenuItem> _items = [];
    public IReadOnlyCollection<MenuItem> Items => _items.AsReadOnly();

    // Navigation
    public Restaurant? Restaurant { get; private set; }

    private MenuCategory() { } // EF Core

    public static MenuCategory Create(Guid restaurantId, string name, int displayOrder = 0)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        return new MenuCategory
        {
            RestaurantId = restaurantId,
            Name         = name.Trim(),
            DisplayOrder = displayOrder
        };
    }

    public void Update(string name, int displayOrder)
    {
        Name         = name.Trim();
        DisplayOrder = displayOrder;
    }

    public void Deactivate() => IsActive = false;
    public void Activate()   => IsActive = true;
}