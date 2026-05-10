namespace RestaurantOrders.Domain.Entities;

using RestaurantOrders.Domain.Common;

/// <summary>
/// Customer entity
/// </summary>
public class Customer : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    
    public static Customer Create(string name, string phone)
    {
        return new Customer
        {
            Name = name ?? throw new ArgumentNullException(nameof(name)),
            Phone = phone ?? throw new ArgumentNullException(nameof(phone))
        };
    }
}
