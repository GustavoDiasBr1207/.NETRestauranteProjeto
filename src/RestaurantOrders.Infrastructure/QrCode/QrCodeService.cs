namespace RestaurantOrders.Infrastructure.QrCode;

using QRCoder;
using RestaurantOrders.Domain.Interfaces.Services;
using RestaurantOrders.Domain.Interfaces.Repositories;
using RestaurantOrders.Domain.Exceptions;

public class QrCodeService(ITableRepository tableRepository) : IQrCodeService
{
    public Task<byte[]> GenerateAsync(Guid tableId, CancellationToken ct = default)
    {
        // Gera QR Code com o token da mesa como conteúdo
        var content   = tableId.ToString();
        using var gen  = new QRCodeGenerator();
        var data      = gen.CreateQrCode(content, QRCodeGenerator.ECCLevel.Q);
        using var code = new PngByteQRCode(data);
        return Task.FromResult(code.GetGraphic(20));
    }

    public async Task<bool> ValidateAsync(string token, CancellationToken ct = default)
    {
        var table = await tableRepository.GetByQrCodeTokenAsync(token, ct);
        return table is { IsActive: true };
    }
}
