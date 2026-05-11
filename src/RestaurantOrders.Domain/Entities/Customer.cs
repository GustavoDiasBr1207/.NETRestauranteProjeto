using RestaurantOrders.Domain.Common;

namespace RestaurantOrders.Domain.Entities;

public class Customer : BaseEntity
{
    public string? Name  { get; private set; }
    public string? Phone { get; private set; }

    private Customer() { } // EF Core

    public static Customer Create(string? name = null, string? phone = null) =>
        new Customer
        {
            Name  = name?.Trim(),
            Phone = phone?.Trim()
        };

    public void Update(string? name, string? phone)
    {
        Name  = name?.Trim();
        Phone = phone?.Trim();
    }
}