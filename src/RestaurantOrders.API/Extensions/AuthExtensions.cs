namespace RestaurantOrders.API.Extensions;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

public static class AuthExtensions
{
    public static IServiceCollection AddAuth(this IServiceCollection services, IConfiguration configuration)
    {
        var supabaseUrl = configuration["Supabase:Url"]
            ?? throw new InvalidOperationException("Supabase:Url não configurado.");

        var jwtSecret = configuration["Supabase:JwtSecret"];

        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                // Opção 1 — validação via OIDC discovery do Supabase (padrão)
                options.Authority = $"{supabaseUrl}/auth/v1";
                options.Audience  = "authenticated";

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer           = true,
                    ValidateAudience         = true,
                    ValidateLifetime         = true,
                    ValidateIssuerSigningKey = true,
                };

                // Opção 2 — validação direta pelo JWT Secret do Supabase (descomente se preferir)
                // if (!string.IsNullOrEmpty(jwtSecret))
                // {
                //     options.Authority = null;
                //     options.TokenValidationParameters = new TokenValidationParameters
                //     {
                //         ValidateIssuerSigningKey = true,
                //         IssuerSigningKey         = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret)),
                //         ValidateAudience         = false,
                //         ValidateIssuer           = false,
                //     };
                // }

                options.RequireHttpsMetadata = false; // defina true em produção
            });

        services.AddAuthorization();
        return services;
    }
}
