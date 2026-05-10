namespace RestaurantOrders.Domain.Entities;

using RestaurantOrders.Domain.Common;
using RestaurantOrders.Domain.Enums;
using RestaurantOrders.Domain.Events;
using RestaurantOrders.Domain.Exceptions;
using RestaurantOrders.Domain.ValueObjects;

/// <summary>
/// Order aggregate root - contains all business logic for order operations
/// </summary>
public class Order : BaseEntity
{
    public Guid RestaurantId { get; set; }
    public Guid TableId { get; set; }
    public Guid? CustomerId { get; set; }
    public OrderStatusEnum Status { get; private set; }
    public Money TotalAmount { get; private set; } = null!;
    public DateTime? SubmittedAt { get; private set; }
    
    private readonly List<OrderItem> _items = new();
    public IReadOnlyList<OrderItem> Items => _items.AsReadOnly();
    
    public static Order Create(Guid restaurantId, Guid tableId, Guid? customerId = null)
    {
        var order = new Order
        {
            RestaurantId = restaurantId,
            TableId = tableId,
            CustomerId = customerId,
            Status = OrderStatusEnum.Draft,
            TotalAmount = new Money(0)
        };
        
        order.AddDomainEvent(new OrderPlacedEvent(order.Id, restaurantId, tableId, order.TotalAmount));
        
        return order;
    }
    
    public void AddItem(MenuItem menuItem, int quantity, string? notes = null)
    {
        if (menuItem == null)
            throw new ArgumentNullException(nameof(menuItem));
        
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be greater than 0", nameof(quantity));
        
        if (!menuItem.IsAvailable)
            throw new MenuItemUnavailableException($"Menu item '{menuItem.Name}' is not available");
        
        if (Status != OrderStatusEnum.Draft)
            throw new InvalidOrderStatusException($"Cannot add item to order in '{Status}' status");
        
        var existingItem = _items.FirstOrDefault(i => i.MenuItemId == menuItem.Id);
        
        if (existingItem != null)
        {
            existingItem.UpdateQuantity(quantity);
        }
        else
        {
            var orderItem = OrderItem.Create(menuItem.Id, menuItem.Price, quantity, notes);
            _items.Add(orderItem);
        }
        
        CalculateTotalAmount();
        
        this.AddDomainEvent(new OrderItemAddedEvent(this.Id, menuItem.Id, quantity));
    }
    
    public void RemoveItem(Guid orderItemId)
    {
        if (Status != OrderStatusEnum.Draft)
            throw new InvalidOrderStatusException($"Cannot remove item from order in '{Status}' status");
        
        var item = _items.FirstOrDefault(i => i.Id == orderItemId);
        if (item == null)
            throw new NotFoundException($"Order item with id '{orderItemId}' not found");
        
        _items.Remove(item);
        CalculateTotalAmount();
    }
    
    public void Submit()
    {
        if (Status != OrderStatusEnum.Draft)
            throw new InvalidOrderStatusException($"Cannot submit order in '{Status}' status");
        
        if (_items.Count == 0)
            throw new InvalidOrderStatusException("Cannot submit order without items");
        
        Status = OrderStatusEnum.Pending;
        SubmittedAt = DateTime.UtcNow;
        
        this.AddDomainEvent(new OrderStatusChangedEvent(this.Id, OrderStatusEnum.Draft, OrderStatusEnum.Pending));
    }
    
    public void Confirm()
    {
        if (Status != OrderStatusEnum.Pending)
            throw new InvalidOrderStatusException($"Cannot confirm order from '{Status}' status");
        
        Status = OrderStatusEnum.Confirmed;
        
        this.AddDomainEvent(new OrderStatusChangedEvent(this.Id, OrderStatusEnum.Pending, OrderStatusEnum.Confirmed));
    }
    
    public void MarkAsReady()
    {
        if (Status != OrderStatusEnum.Preparing)
            throw new InvalidOrderStatusException($"Cannot mark order as ready from '{Status}' status");
        
        Status = OrderStatusEnum.Ready;
        
        this.AddDomainEvent(new OrderStatusChangedEvent(this.Id, OrderStatusEnum.Preparing, OrderStatusEnum.Ready));
    }
    
    public void Deliver()
    {
        if (Status != OrderStatusEnum.Ready)
            throw new InvalidOrderStatusException($"Cannot deliver order from '{Status}' status");
        
        Status = OrderStatusEnum.Delivered;
        
        this.AddDomainEvent(new OrderStatusChangedEvent(this.Id, OrderStatusEnum.Ready, OrderStatusEnum.Delivered));
    }
    
    public void Cancel()
    {
        if (Status == OrderStatusEnum.Delivered || Status == OrderStatusEnum.Cancelled)
            throw new InvalidOrderStatusException($"Cannot cancel order in '{Status}' status");
        
        var previousStatus = Status;
        Status = OrderStatusEnum.Cancelled;
        
        this.AddDomainEvent(new OrderStatusChangedEvent(this.Id, previousStatus, OrderStatusEnum.Cancelled));
    }
    
    private void CalculateTotalAmount()
    {
        if (_items.Count == 0)
        {
            TotalAmount = new Money(0);
            return;
        }
        
        var total = _items
            .Select(item => new Money(item.Subtotal.Amount, item.Subtotal.Currency))
            .Aggregate((acc, item) => acc + item);
        
        TotalAmount = total;
    }
}
