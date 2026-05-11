using RestaurantOrders.Domain.Common;
using RestaurantOrders.Domain.Enums;
using RestaurantOrders.Domain.Events;
using RestaurantOrders.Domain.Exceptions;
using RestaurantOrders.Domain.ValueObjects;

namespace RestaurantOrders.Domain.Entities;

public class Order : BaseEntity
{
    public Guid             RestaurantId { get; private set; }
    public Guid             TableId      { get; private set; }
    public Guid?            CustomerId   { get; private set; }
    public OrderStatusEnum  Status       { get; private set; }
    public Money            TotalAmount  { get; private set; } = null!;
    public string?          Notes        { get; private set; }
    public DateTime?        PlacedAt     { get; private set; }
    public DateTime?        ConfirmedAt  { get; private set; }
    public DateTime?        ReadyAt      { get; private set; }
    public DateTime?        DeliveredAt  { get; private set; }

    // Navigations
    public Table?      Table      { get; private set; }
    public Restaurant? Restaurant { get; private set; }
    public Customer?   Customer   { get; private set; }

    private readonly List<OrderItem> _items = [];
    public IReadOnlyCollection<OrderItem> Items => _items.AsReadOnly();

    private Order() { } // EF Core

    // ── Factory ──────────────────────────────────────────────────────────────

    public static Order Create(Guid restaurantId, Guid tableId, Guid? customerId = null, string? notes = null)
    {
        var order = new Order
        {
            RestaurantId = restaurantId,
            TableId      = tableId,
            CustomerId   = customerId,
            Notes        = notes?.Trim(),
            Status       = OrderStatusEnum.Draft,
            TotalAmount  = Money.Zero()
        };

        return order;
    }

    // ── Itens ─────────────────────────────────────────────────────────────────

    public void AddItem(MenuItem menuItem, int quantity, string? notes = null)
    {
        EnsureStatus(OrderStatusEnum.Draft, "adicionar itens");

        if (!menuItem.IsAvailable)
            throw new MenuItemUnavailableException(menuItem.Id);

        var existing = _items.FirstOrDefault(i => i.MenuItemId == menuItem.Id);
        if (existing is not null)
            existing.UpdateQuantity(existing.Quantity + quantity);
        else
            _items.Add(OrderItem.Create(Id, menuItem, quantity, notes));

        RecalculateTotal();
        AddDomainEvent(new OrderItemAddedEvent(Id, menuItem.Id, quantity));
    }

    public void RemoveItem(Guid orderItemId)
    {
        EnsureStatus(OrderStatusEnum.Draft, "remover itens");

        var item = _items.FirstOrDefault(i => i.Id == orderItemId)
            ?? throw new DomainException($"Item '{orderItemId}' não encontrado no pedido.");

        _items.Remove(item);
        RecalculateTotal();
    }

    public void UpdateNotes(string? notes)
    {
        EnsureStatus(OrderStatusEnum.Draft, "atualizar observações");
        Notes = notes?.Trim();
    }

    // ── Transições de status ──────────────────────────────────────────────────

    /// <summary>Cliente envia o pedido para a cozinha.</summary>
    public void Submit()
    {
        EnsureStatus(OrderStatusEnum.Draft, "enviar");

        if (_items.Count == 0)
            throw new DomainException("O pedido precisa ter ao menos um item antes de ser enviado.");

        Status   = OrderStatusEnum.Pending;
        PlacedAt = DateTime.UtcNow;

        AddDomainEvent(new OrderPlacedEvent(Id, RestaurantId, TableId, TotalAmount));
    }

    /// <summary>Cozinha confirma que recebeu o pedido.</summary>
    public void Confirm()
    {
        EnsureStatus(OrderStatusEnum.Pending, "confirmar");
        Status      = OrderStatusEnum.Confirmed;
        ConfirmedAt = DateTime.UtcNow;
        AddDomainEvent(new OrderStatusChangedEvent(Id, OrderStatusEnum.Pending, OrderStatusEnum.Confirmed));
    }

    /// <summary>Cozinha começa a preparar.</summary>
    public void StartPreparing()
    {
        EnsureStatus(OrderStatusEnum.Confirmed, "iniciar preparo");
        Status = OrderStatusEnum.Preparing;
        AddDomainEvent(new OrderStatusChangedEvent(Id, OrderStatusEnum.Confirmed, OrderStatusEnum.Preparing));
    }

    /// <summary>Cozinha marca como pronto para retirada/entrega.</summary>
    public void MarkReady()
    {
        EnsureStatus(OrderStatusEnum.Preparing, "marcar como pronto");
        Status  = OrderStatusEnum.Ready;
        ReadyAt = DateTime.UtcNow;
        AddDomainEvent(new OrderStatusChangedEvent(Id, OrderStatusEnum.Preparing, OrderStatusEnum.Ready));
    }

    /// <summary>Garçom entrega na mesa.</summary>
    public void Deliver()
    {
        EnsureStatus(OrderStatusEnum.Ready, "entregar");
        Status      = OrderStatusEnum.Delivered;
        DeliveredAt = DateTime.UtcNow;
        AddDomainEvent(new OrderStatusChangedEvent(Id, OrderStatusEnum.Ready, OrderStatusEnum.Delivered));
    }

    /// <summary>Cancela o pedido (apenas enquanto não entregue).</summary>
    public void Cancel()
    {
        if (Status is OrderStatusEnum.Delivered or OrderStatusEnum.Cancelled)
            throw new InvalidOrderStatusException(Status, OrderStatusEnum.Cancelled);

        var previous = Status;
        Status = OrderStatusEnum.Cancelled;
        AddDomainEvent(new OrderStatusChangedEvent(Id, previous, OrderStatusEnum.Cancelled));
    }

    // ── Helpers ───────────────────────────────────────────────────────────────

    private void RecalculateTotal() =>
        TotalAmount = _items.Aggregate(Money.Zero(), (acc, i) => acc + i.Subtotal);

    private void EnsureStatus(OrderStatusEnum expected, string action)
    {
        if (Status != expected)
            throw new DomainException(
                $"Não é possível {action} um pedido com status '{Status}'. Status esperado: '{expected}'.");
    }
}