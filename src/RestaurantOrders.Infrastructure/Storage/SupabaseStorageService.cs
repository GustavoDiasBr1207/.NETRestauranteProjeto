namespace RestaurantOrders.Infrastructure.Storage;

using Microsoft.Extensions.Logging;
using RestaurantOrders.Domain.Interfaces.Services;

// Implemente aqui o upload real para Supabase Storage quando adicionar as credenciais.
public class SupabaseStorageService(ILogger<SupabaseStorageService> logger) : IStorageService
{
    public Task<string> UploadAsync(Stream file, string fileName, string bucket, CancellationToken ct = default)
    {
        logger.LogWarning("SupabaseStorageService.UploadAsync não implementado. Configure o cliente Supabase.");
        // TODO: return await supabaseClient.Storage.From(bucket).Upload(file, fileName)
        return Task.FromResult(string.Empty);
    }

    public Task<bool> DeleteAsync(string filePath, string bucket, CancellationToken ct = default)
    {
        logger.LogWarning("SupabaseStorageService.DeleteAsync não implementado.");
        // TODO: return await supabaseClient.Storage.From(bucket).Remove([filePath])
        return Task.FromResult(false);
    }
}
