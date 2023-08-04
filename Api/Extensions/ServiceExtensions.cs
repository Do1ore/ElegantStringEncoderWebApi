using Microsoft.OpenApi.Models;

namespace Api.Extensions;

public static class ServiceExtensions
{
    public static void AddAndConfigureSwaggerGen(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo { Title = "StringEncoder API v1", Version = "v1" });
            options.AddSignalRSwaggerGen();
        });
    }

    public static void ConfigureCorsPolicy(this IServiceCollection services)
    {
        services.AddCors(setup =>
        {
            setup.AddPolicy("angular", builder =>
            {
                {
                    builder.WithOrigins("http://localhost:4200")
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                }
            });
        });
    }
}