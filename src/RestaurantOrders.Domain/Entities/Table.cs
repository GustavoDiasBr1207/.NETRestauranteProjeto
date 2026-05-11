using RestaurantOrders.Domain.Common;
using RestaurantOrders.Domain.ValueObjects;

namespace RestaurantOrders.Domain.Entities;

public class Table : BaseEntity
{
    public Guid        RestaurantId { get; private set; }
    public int         Number       { get; private set; }
    public TableQrCode QrCode       { get; private set; } = null!;
    public string?     QrCodeUrl    { get; private set; }
    public bool        IsActive     { get; private set; } = true;

    // Navigation
    public Restaurant? Restaurant { get; private set; }

    private Table() { } // EF Core

    public static Table Create(Guid restaurantId, int number)
    {
        if (number <= 0)
            throw new ArgumentException("Número da mesa deve ser maior que zero.", nameof(number));

        return new Table
        {
            RestaurantId = restaurantId,
            Number       = number,
            QrCode       = TableQrCode.Generate()
        };
    }

    public void RegenerateQrCode() => QrCode = TableQrCode.Generate();

    public void SetQrCodeUrl(string url) => QrCodeUrl = url;

    public void Deactivate() => IsActive = false;
    public void Activate()   => IsActive = true;
}