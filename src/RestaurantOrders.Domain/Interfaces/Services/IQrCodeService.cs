namespace RestaurantOrders.Domain.Interfaces.Services;

/// <summary>
/// Service interface for QR code operations
/// </summary>
public interface IQrCodeService
{
    Task<byte[]> GenerateAsync(Guid tableId, CancellationToken cancellationToken = default);
    Task<bool> ValidateAsync(string token, CancellationToken cancellationToken = default);
}
