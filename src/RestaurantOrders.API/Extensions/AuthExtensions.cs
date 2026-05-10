namespace RestaurantOrders.API.Extensions;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

/// <summary>
/// JWT Authentication extensions
/// </summary>
public static class AuthExtensions
{
    public static IServiceCollection AddAuth(this IServiceCollection services)
    {
        // TODO: Configure JWT authentication from Supabase or config
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                // TODO: Set up JWT bearer options
            });
        
        return services;
    }
}
