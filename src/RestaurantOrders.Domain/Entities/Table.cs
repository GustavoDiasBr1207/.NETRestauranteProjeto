namespace RestaurantOrders.Domain.Entities;

using RestaurantOrders.Domain.Common;
using RestaurantOrders.Domain.ValueObjects;

/// <summary>
/// Table entity with QR code token
/// </summary>
public class Table : BaseEntity
{
    public Guid RestaurantId { get; set; }
    public int Number { get; set; }
    public string QrCodeToken { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    
    public static Table Create(Guid restaurantId, int number)
    {
        return new Table
        {
            RestaurantId = restaurantId,
            Number = number,
            QrCodeToken = TableQrCode.Generate().Token,
            IsActive = true
        };
    }
}
