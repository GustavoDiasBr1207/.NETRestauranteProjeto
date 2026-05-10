namespace RestaurantOrders.Domain.Interfaces.Services;

/// <summary>
/// Service interface for file storage operations
/// </summary>
public interface IStorageService
{
    Task<string> UploadAsync(Stream file, string fileName, string bucket, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(string filePath, string bucket, CancellationToken cancellationToken = default);
}
