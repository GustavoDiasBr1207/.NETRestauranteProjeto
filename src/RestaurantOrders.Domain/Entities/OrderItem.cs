namespace RestaurantOrders.Domain.Entities;

using RestaurantOrders.Domain.Common;
using RestaurantOrders.Domain.ValueObjects;

/// <summary>
/// Order item entity - part of Order aggregate
/// </summary>
public class OrderItem : BaseEntity
{
    public Guid OrderId { get; set; }
    public Guid MenuItemId { get; set; }
    public Money UnitPrice { get; set; } = null!;
    public int Quantity { get; set; }
    public Money Subtotal { get; set; } = null!;
    public string? Notes { get; set; }
    
    public static OrderItem Create(Guid menuItemId, Money unitPrice, int quantity, string? notes = null)
    {
        var subtotal = new Money(unitPrice.Amount * quantity, unitPrice.Currency);
        
        return new OrderItem
        {
            MenuItemId = menuItemId,
            UnitPrice = unitPrice,
            Quantity = quantity,
            Subtotal = subtotal,
            Notes = notes
        };
    }
    
    public void UpdateQuantity(int quantity)
    {
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be greater than 0", nameof(quantity));
        
        Quantity = quantity;
        Subtotal = new Money(UnitPrice.Amount * Quantity, UnitPrice.Currency);
    }
}
