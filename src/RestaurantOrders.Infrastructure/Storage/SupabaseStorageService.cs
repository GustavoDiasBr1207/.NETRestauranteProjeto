namespace RestaurantOrders.Infrastructure.Storage;

using RestaurantOrders.Domain.Interfaces.Services;

/// <summary>
/// Storage service implementation using Supabase Storage
/// </summary>
public class SupabaseStorageService : IStorageService
{
    public async Task<string> UploadAsync(Stream file, string fileName, string bucket, CancellationToken cancellationToken = default)
    {
        // TODO: Implement file upload to Supabase Storage
        return string.Empty;
    }
    
    public async Task<bool> DeleteAsync(string filePath, string bucket, CancellationToken cancellationToken = default)
    {
        // TODO: Implement file deletion from Supabase Storage
        return false;
    }
}
