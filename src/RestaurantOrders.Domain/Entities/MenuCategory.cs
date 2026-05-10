namespace RestaurantOrders.Domain.Entities;

using RestaurantOrders.Domain.Common;

/// <summary>
/// Menu category entity
/// </summary>
public class MenuCategory : BaseEntity
{
    public Guid RestaurantId { get; set; }
    public string Name { get; set; } = string.Empty;
    public int DisplayOrder { get; set; }
    public bool IsActive { get; set; }
    
    public static MenuCategory Create(Guid restaurantId, string name, int displayOrder)
    {
        return new MenuCategory
        {
            RestaurantId = restaurantId,
            Name = name ?? throw new ArgumentNullException(nameof(name)),
            DisplayOrder = displayOrder,
            IsActive = true
        };
    }
}
