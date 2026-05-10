namespace RestaurantOrders.Application;

using Microsoft.Extensions.DependencyInjection;
using AutoMapper;
using MediatR;

/// <summary>
/// Dependency injection configuration for Application layer
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // Register MediatR
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));
        
        // Register AutoMapper
        services.AddAutoMapper(typeof(DependencyInjection).Assembly);
        
        // Register FluentValidation
        services.AddFluentValidation(cfg =>
        {
            cfg.RegisterValidatorsFromAssembly(typeof(DependencyInjection).Assembly);
        });
        
        return services;
    }
}
