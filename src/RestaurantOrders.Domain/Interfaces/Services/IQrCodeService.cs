namespace RestaurantOrders.Domain.Interfaces.Services;

/// <summary>Geração e validação de QR Codes para mesas.</summary>
public interface IQrCodeService
{
    /// <summary>Gera a imagem PNG do QR Code para a mesa informada.</summary>
    Task<byte[]> GenerateAsync(Guid tableId, CancellationToken ct = default);

    /// <summary>Verifica se o token do QR Code é válido e corresponde a uma mesa ativa.</summary>
    Task<bool> ValidateAsync(string token, CancellationToken ct = default);
}
