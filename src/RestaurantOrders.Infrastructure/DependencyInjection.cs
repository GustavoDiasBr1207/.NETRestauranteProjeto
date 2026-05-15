namespace RestaurantOrders.Infrastructure;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using RestaurantOrders.Domain.Interfaces;
using RestaurantOrders.Domain.Interfaces.Repositories;
using RestaurantOrders.Domain.Interfaces.Services;
using RestaurantOrders.Infrastructure.Persistence;
using RestaurantOrders.Infrastructure.Persistence.Repositories;
using RestaurantOrders.Infrastructure.QrCode;
using RestaurantOrders.Infrastructure.Realtime;
using RestaurantOrders.Infrastructure.Storage;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' não configurada.");

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(connectionString, npgsql =>
            {
                npgsql.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName);
                npgsql.EnableRetryOnFailure(maxRetryCount: 3, maxRetryDelay: TimeSpan.FromSeconds(5), errorCodesToAdd: null);
                npgsql.CommandTimeout(30);
            }));

        // IUnitOfWork resolvido a partir do mesmo ApplicationDbContext do escopo
        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<ApplicationDbContext>());

        // Repositórios
        services.AddScoped<IOrderRepository,      OrderRepository>();
        services.AddScoped<IMenuRepository,       MenuRepository>();
        services.AddScoped<ITableRepository,      TableRepository>();
        services.AddScoped<IRestaurantRepository, RestaurantRepository>();

        // Serviços de infraestrutura
        services.AddScoped<INotificationService, SupabaseNotificationService>();
        services.AddScoped<IQrCodeService,       QrCodeService>();
        services.AddScoped<IStorageService,      SupabaseStorageService>();

        // Supabase Client — descomente e configure quando tiver as credenciais:
        // var url     = configuration["Supabase:Url"]!;
        // var anonKey = configuration["Supabase:AnonKey"]!;
        // var client  = new Supabase.Client(url, anonKey, new Supabase.SupabaseOptions { AutoConnectRealtime = true });
        // await client.InitializeAsync();
        // services.AddSingleton(client);

        return services;
    }
}
