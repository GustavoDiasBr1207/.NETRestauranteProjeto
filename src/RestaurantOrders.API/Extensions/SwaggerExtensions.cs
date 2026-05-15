namespace RestaurantOrders.API.Extensions;

using System.Reflection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using RestaurantOrders.API.Models;
using RestaurantOrders.Application.Common.DTOs;

public static class SwaggerExtensions
{
    public static IServiceCollection AddSwagger(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();

        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title   = "Restaurant Orders API",
                Version = "v1",
                Description =
                    "Sistema de pedidos via QR Code: o cliente escaneia a mesa, " +
                    "monta o carrinho e a cozinha recebe os pedidos em tempo real " +
                    "via Supabase Realtime.",
                Contact = new OpenApiContact
                {
                    Name  = "RestaurantOrders",
                    Email = "dev@restaurantorders.com"
                }
            });

            // ── Autenticação JWT (Supabase) ──────────────────────────────────
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name         = "Authorization",
                Type         = SecuritySchemeType.Http,
                Scheme       = "bearer",
                BearerFormat = "JWT",
                In           = ParameterLocation.Header,
                Description  =
                    "Token JWT emitido pelo Supabase Auth.\n\n" +
                    "Formato: **Bearer {token}**\n\n" +
                    "Obtenha o token em: `POST {supabase_url}/auth/v1/token`"
            });

            // Aplica o requisito de segurança apenas nos endpoints [Authorize]
            options.OperationFilter<AuthorizeOperationFilter>();

            // ── XML comments — API ───────────────────────────────────────────
            var apiXml = Path.Combine(AppContext.BaseDirectory,
                $"{Assembly.GetExecutingAssembly().GetName().Name}.xml");
            if (File.Exists(apiXml))
                options.IncludeXmlComments(apiXml, includeControllerXmlComments: true);

            // ── XML comments — Application (DTOs, Commands) ──────────────────
            var appXml = Path.Combine(AppContext.BaseDirectory,
                $"{typeof(OrderDto).Assembly.GetName().Name}.xml");
            if (File.Exists(appXml))
                options.IncludeXmlComments(appXml);

            // ── Enum como string no schema ────────────────────────────────────
            options.UseInlineDefinitionsForEnums();

            // ── Descrições das tags (grupos de endpoints) ─────────────────────
            options.DocumentFilter<TagDescriptionsFilter>();
        });

        return services;
    }

    public static IApplicationBuilder UseSwaggerWithUI(this IApplicationBuilder app)
    {
        app.UseSwagger(c =>
        {
            c.RouteTemplate = "openapi/{documentName}/openapi.json";
        });

        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/openapi/v1/openapi.json", "Restaurant Orders API v1");
            c.RoutePrefix        = "docs";
            c.DocumentTitle      = "Restaurant Orders — API Docs";
            c.DefaultModelsExpandDepth(1);
            c.DefaultModelExpandDepth(3);
            c.DisplayRequestDuration();
            c.EnableFilter();
            c.EnableTryItOutByDefault();
        });

        return app;
    }
}

// ── Aplica [Authorize] como requisito de segurança por operação ───────────────
file sealed class AuthorizeOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var hasAuthorize =
            context.MethodInfo.GetCustomAttributes<AuthorizeAttribute>(true).Any() ||
            (context.MethodInfo.DeclaringType?
                .GetCustomAttributes<AuthorizeAttribute>(true).Any() ?? false);

        var hasAllowAnonymous =
            context.MethodInfo.GetCustomAttributes<AllowAnonymousAttribute>(true).Any();

        if (!hasAuthorize || hasAllowAnonymous)
            return;

        operation.Security =
        [
            new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id   = "Bearer"
                        }
                    },
                    []
                }
            }
        ];
    }
}

// ── Filtro interno: descrições de tags ───────────────────────────────────────
file sealed class TagDescriptionsFilter : IDocumentFilter
{
    public void Apply(OpenApiDocument doc, DocumentFilterContext _)
    {
        doc.Tags =
        [
            new OpenApiTag
            {
                Name        = "Orders",
                Description = "Criação e ciclo de vida dos pedidos — do carrinho à entrega."
            },
            new OpenApiTag
            {
                Name        = "Menu",
                Description = "Consulta do cardápio por restaurante, usada após leitura do QR Code."
            },
            new OpenApiTag
            {
                Name        = "Tables",
                Description = "Resolução de mesa a partir do token do QR Code escaneado."
            },
            new OpenApiTag
            {
                Name        = "MenuItems",
                Description = "Gestão de itens do cardápio (criação e disponibilidade). Requer autenticação."
            },
            new OpenApiTag
            {
                Name        = "Restaurants",
                Description = "Informações gerais do restaurante."
            }
        ];
    }
}
