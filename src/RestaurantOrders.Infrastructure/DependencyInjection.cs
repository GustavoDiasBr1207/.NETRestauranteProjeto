namespace RestaurantOrders.Infrastructure;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using RestaurantOrders.Infrastructure.Persistence;
using RestaurantOrders.Infrastructure.Persistence.Repositories;
using RestaurantOrders.Infrastructure.Realtime;
using RestaurantOrders.Infrastructure.QrCode;
using RestaurantOrders.Infrastructure.Storage;
using RestaurantOrders.Domain.Interfaces.Repositories;
using RestaurantOrders.Domain.Interfaces.Services;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseNpgsql(connectionString, npgsqlOptions =>
            {
                npgsqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 3,
                    maxRetryDelay: TimeSpan.FromSeconds(5),
                    errorCodesToAdd: null);
                npgsqlOptions.CommandTimeout(30);
            });
        });

        // Repositories
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IMenuRepository, MenuRepository>();
        services.AddScoped<ITableRepository, TableRepository>();
        services.AddScoped<IRestaurantRepository, RestaurantRepository>();

        // Services
        services.AddScoped<INotificationService, SupabaseNotificationService>();
        services.AddScoped<IQrCodeService, QrCodeService>();
        services.AddScoped<IStorageService, SupabaseStorageService>();

        return services;
    }
}