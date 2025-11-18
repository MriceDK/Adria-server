using System.Data.Common;
using Adria.Application.Contracts;
using Adria.Domain.Subcriptions;
using Adria.Domain.Users;
using Adria.Infrastructure.Persistence.Queries;
using Adria.Infrastructure.Persistence.Repositories;

namespace Adria.Main.Modules.Persistence;

public static class PersistenceModule
{
    private static string _connectionString = string.Empty;

    public static IServiceCollection AddPersistenceModule(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        _connectionString = configuration["Persistence:ConnectionString"]!;
        return services
            .AddAdoServices(configuration)
            .AddRepositories()
            .AddQueries();
    }

    public static WebApplication UsePersistenceModule(this WebApplication app)
    {
        return app.ApplyMigrations();
    }

    private static IServiceCollection AddRepositories(
        this IServiceCollection services
    )
    {
        // Configure repositories here.
        return services
                .AddScoped<ISubscriptionRepository, AdoSubscriptionRepository>(serviceProvider =>
                {
                    return new AdoSubscriptionRepository(
                        serviceProvider.GetRequiredService<DbProviderFactory>(),
                        _connectionString,
                        serviceProvider.GetRequiredService<ILogger<AdoSubscriptionRepository>>()
                    );
                })
                .AddScoped<IUserRepository, AdoUserRepository>(serviceProvider =>
                {
                    var factory = serviceProvider.GetRequiredService<DbProviderFactory>();
                    return new AdoUserRepository(
                        factory,
                        _connectionString,
                        serviceProvider.GetRequiredService<ILogger<AdoUserRepository>>(),
                        subscriptionRepository: new AdoSubscriptionRepository(factory,
                            _connectionString,
                            serviceProvider.GetRequiredService<ILogger<AdoSubscriptionRepository>>()
                        )
                    );
                })
            ;
    }

    private static IServiceCollection AddQueries(
        this IServiceCollection services
    )
    {
        // Configure queries here.
        return services
                .AddScoped<IAllSubscriptionsQuery, AllSubscriptionsQuery>(serviceProvider =>
                {
                    return new AllSubscriptionsQuery(
                        serviceProvider.GetRequiredService<DbProviderFactory>(),
                        _connectionString,
                        serviceProvider.GetRequiredService<ILogger<AllSubscriptionsQuery>>()
                    );
                })
                .AddScoped<ISubscriptionByIdQuery, SubscriptionByIdQuery>(serviceProvider =>
                {
                    return new SubscriptionByIdQuery(
                        serviceProvider.GetRequiredService<DbProviderFactory>(),
                        _connectionString,
                        serviceProvider.GetRequiredService<ILogger<SubscriptionByIdQuery>>()
                    );
                })
                .AddScoped<IUserByIdQuery, UserByIdQuery>(serviceProvider =>
                {
                    return new UserByIdQuery(
                        serviceProvider.GetRequiredService<DbProviderFactory>(),
                        _connectionString,
                        serviceProvider.GetRequiredService<ILogger<UserByIdQuery>>()
                    );
                })
                .AddScoped<IAllUsersQuery, AllUsersQuery>(serviceProvider =>
                {
                    return new AllUsersQuery(
                        serviceProvider.GetRequiredService<DbProviderFactory>(),
                        _connectionString,
                        serviceProvider.GetRequiredService<ILogger<AllUsersQuery>>()
                    );
                })
            ;
    }

    private static IServiceCollection AddAdoServices(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        string provider = configuration["Persistence:Provider"]!;

        DbProviderFactories.RegisterFactory(provider, MySql.Data.MySqlClient.MySqlClientFactory.Instance);

        return services.AddScoped(serviceProvider => { return DbProviderFactories.GetFactory(provider); });
    }

    private static WebApplication ApplyMigrations(this WebApplication app)
    {
        IServiceProvider serviceProvider = app.Services.CreateScope().ServiceProvider;
        DbProviderFactory factory = serviceProvider.GetRequiredService<DbProviderFactory>();
        string connectionString = serviceProvider.GetRequiredService<IConfiguration>()["Persistence:ConnectionString"]!;

        using DbConnection connection = factory.CreateConnection()!;
        connection.ConnectionString = connectionString;
        connection.Open();

        string scriptPath = Path.Combine(
            AppContext.BaseDirectory,
            "Persistence/Scripts",
            "create_database.sql"
        );

        using DbCommand command = connection.CreateCommand();
        command.CommandText = File.ReadAllText(scriptPath);
        command.ExecuteNonQuery();
        return app;
    }
}