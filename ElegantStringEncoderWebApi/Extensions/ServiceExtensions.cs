using Microsoft.OpenApi.Models;

namespace ElegantStringEncoderWebApi.Extensions;

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
}