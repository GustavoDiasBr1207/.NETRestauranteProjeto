namespace RestaurantOrders.Domain.Interfaces.Services;

/// <summary>Armazenamento de arquivos no Supabase Storage.</summary>
public interface IStorageService
{
    /// <summary>Faz upload de um arquivo e retorna a URL pública.</summary>
    Task<string> UploadAsync(Stream file, string fileName, string bucket, CancellationToken ct = default);

    /// <summary>Remove um arquivo do bucket informado.</summary>
    Task<bool> DeleteAsync(string filePath, string bucket, CancellationToken ct = default);
}
