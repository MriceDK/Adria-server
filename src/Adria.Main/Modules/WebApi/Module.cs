using System.ComponentModel.DataAnnotations;
using Adria.Domain.Shared.Exceptions;
using Adria.Infrastructure.WebApi;
using Microsoft.OpenApi.Models;

namespace Adria.Main.Modules.WebApi;

public static class WebApiModule
{
    public static IServiceCollection AddWebApiModule(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services
            .AddEndpointsApiExplorer()
            .AddSwaggerGen(options =>
            {
                options.SwaggerDoc(
                    configuration["WebApi:Version"],
                    configuration.BuildOpenApiInfo()
                );
            })
            .AddProblemDetails(options =>
            {
                options.CustomizeProblemDetails = context =>
                {
                    (context.ProblemDetails.Status, context.ProblemDetails.Title, context.HttpContext.Response.StatusCode) = context.Exception switch
                    {
                        ValidationException or ArgumentException or ArgumentNullException
                            => (StatusCodes.Status400BadRequest, "Validation Error", StatusCodes.Status400BadRequest),
                        ElementNotFoundException
                            => (StatusCodes.Status404NotFound, "Not Found", StatusCodes.Status404NotFound),
                        _ => (context.ProblemDetails.Status, context.ProblemDetails.Title, StatusCodes.Status500InternalServerError)
                    };
                };
            })
            .AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    // NOSONAR: permissive CORS is intentional
                    builder
                        .WithExposedHeaders("*") // NOSONAR
                        .AllowAnyHeader() // NOSONAR
                        .AllowAnyMethod() // NOSONAR
                        .AllowAnyOrigin(); // NOSONAR
                });
            })
            .AddHealthChecks();

        return services;
    }

    public static WebApplication UseWebApiModule(this WebApplication app)
    {
        app.UseCors();
        if (!app.Environment.IsProduction())
            app.UseSwagger().UseSwaggerUI();

        app
            .UseHttpsRedirection();

        app.MapHealthChecks("/health");

        return app.MapRoutes();
    }

    private static OpenApiInfo BuildOpenApiInfo(
        this IConfiguration configuration
    )
    {
        return new OpenApiInfo
        {
            Version = configuration["WebApi:Version"],
            Title = configuration["WebApi:Title"],
            Description = configuration["WebApi:Description"],
            Contact = new OpenApiContact
            {
                Name = configuration["WebApi:Contact:Name"],
                Email = configuration["WebApi:Contact:Email"]
            },
        };
    }
}