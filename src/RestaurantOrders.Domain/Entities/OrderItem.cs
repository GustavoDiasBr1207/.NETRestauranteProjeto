using RestaurantOrders.Domain.Common;
using RestaurantOrders.Domain.ValueObjects;

namespace RestaurantOrders.Domain.Entities;

public class OrderItem : BaseEntity
{
    public Guid   OrderId    { get; private set; }
    public Guid   MenuItemId { get; private set; }

    // Snapshot dos dados no momento do pedido — não muda mesmo se o cardápio mudar
    public string Name       { get; private set; } = string.Empty;
    public Money  UnitPrice  { get; private set; } = null!;
    public Money  Subtotal   { get; private set; } = null!;
    public int    Quantity   { get; private set; }
    public string? Notes     { get; private set; }

    // Navigation
    public MenuItem? MenuItem { get; private set; }

    private OrderItem() { } // EF Core

    internal static OrderItem Create(Guid orderId, MenuItem menuItem, int quantity, string? notes = null)
    {
        if (quantity <= 0)
            throw new ArgumentException("Quantidade deve ser maior que zero.", nameof(quantity));

        return new OrderItem
        {
            OrderId    = orderId,
            MenuItemId = menuItem.Id,
            Name       = menuItem.Name,              // snapshot
            UnitPrice  = menuItem.Price,             // snapshot
            Subtotal   = menuItem.Price * quantity,  // snapshot
            Quantity   = quantity,
            Notes      = notes?.Trim()
        };
    }

    internal void UpdateQuantity(int newQuantity)
    {
        if (newQuantity <= 0)
            throw new ArgumentException("Quantidade deve ser maior que zero.", nameof(newQuantity));

        Quantity = newQuantity;
        Subtotal = UnitPrice * newQuantity;
    }
}