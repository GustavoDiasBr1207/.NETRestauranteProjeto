namespace RestaurantOrders.Infrastructure.QrCode;

using RestaurantOrders.Domain.Interfaces.Services;

/// <summary>
/// QR code service implementation using QRCoder
/// </summary>
public class QrCodeService : IQrCodeService
{
    public async Task<byte[]> GenerateAsync(Guid tableId, CancellationToken cancellationToken = default)
    {
        // TODO: Implement QR code generation using QRCoder
        return Array.Empty<byte>();
    }
    
    public async Task<bool> ValidateAsync(string token, CancellationToken cancellationToken = default)
    {
        // TODO: Implement QR code token validation
        return false;
    }
}
