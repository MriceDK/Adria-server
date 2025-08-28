using Microsoft.AspNetCore.Builder;
using Microsoft.OpenApi.Models;

namespace Adria.Infrastructure.WebApi;

public static class Routes
{
    public static OpenApiInfo OpenApiInfo { get; } = new OpenApiInfo
    {
        Version = "v1",
        Title = "Your nice title here",
        Description = "Your even nicer description here",
        Contact = new OpenApiContact
        {
            Name = "Group XX",
            Email = "info@group-XX.adria"
        }
    };

    public static WebApplication MapRoutes(this WebApplication app)
    {
        return app;
    }
}
