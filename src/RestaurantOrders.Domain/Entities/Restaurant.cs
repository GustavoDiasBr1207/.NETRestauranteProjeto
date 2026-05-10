namespace RestaurantOrders.Domain.Entities;

using RestaurantOrders.Domain.Common;

/// <summary>
/// Restaurant aggregate root
/// </summary>
public class Restaurant : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string? LogoUrl { get; set; }
    public bool IsActive { get; set; }
    
    public static Restaurant Create(string name, string slug, string? logoUrl = null)
    {
        return new Restaurant
        {
            Name = name ?? throw new ArgumentNullException(nameof(name)),
            Slug = slug ?? throw new ArgumentNullException(nameof(slug)),
            LogoUrl = logoUrl,
            IsActive = true
        };
    }
}
