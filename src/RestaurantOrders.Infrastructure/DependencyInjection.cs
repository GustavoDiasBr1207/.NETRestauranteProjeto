namespace RestaurantOrders.Infrastructure;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using RestaurantOrders.Infrastructure.Persistence;
using RestaurantOrders.Infrastructure.Persistence.Repositories;
using RestaurantOrders.Infrastructure.Realtime;
using RestaurantOrders.Infrastructure.QrCode;
using RestaurantOrders.Infrastructure.Storage;
using RestaurantOrders.Domain.Interfaces.Repositories;
using RestaurantOrders.Domain.Interfaces.Services;

/// <summary>
/// Dependency injection configuration for Infrastructure layer
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, string connectionString)
    {
        // Register DbContext
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(connectionString));
        
        // Register Repositories
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IMenuRepository, MenuRepository>();
        services.AddScoped<ITableRepository, TableRepository>();
        services.AddScoped<IRestaurantRepository, RestaurantRepository>();
        
        // Register Services
        services.AddScoped<INotificationService, SupabaseNotificationService>();
        services.AddScoped<IQrCodeService, QrCodeService>();
        services.AddScoped<IStorageService, SupabaseStorageService>();
        
        return services;
    }
}
