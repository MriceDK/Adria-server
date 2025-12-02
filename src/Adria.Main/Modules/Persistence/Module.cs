using System.Data.Common;
using Adria.Application.Contracts;
using Adria.Application.Contracts.Data;
using Adria.Application.FoodComposition;
using Adria.Domain.BodyStats;
using Adria.Domain.Food;
using Adria.Domain.Subcriptions;
using Adria.Domain.Users;
using Adria.Infrastructure.Persistence.Queries;
using Adria.Infrastructure.Persistence.Repositories;
using IBodyStatRepository = Adria.Application.Contracts.IBodyStatRepository;

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
            .AddScoped<IFood, AdoFoodRepository>(sp =>
            {
                return new AdoFoodRepository(
                    sp.GetRequiredService<DbProviderFactory>(),
                    _connectionString
                );
            })
            .AddScoped<INutrient, AdoNutrientRepository>(sp =>
            {
                return new AdoNutrientRepository(
                    sp.GetRequiredService<DbProviderFactory>(),
                    _connectionString
                );
            })
            .AddScoped<IFoodComposition, AdoFoodCompositionRepository>(serviceProvider =>
            {
                return new AdoFoodCompositionRepository(
                    serviceProvider.GetRequiredService<DbProviderFactory>(),
                    _connectionString,
                    serviceProvider.GetRequiredService<ILogger<AdoFoodCompositionRepository>>(),
                    serviceProvider.GetRequiredService<IFood>(),
                    serviceProvider.GetRequiredService<INutrient>());
            }).AddScoped<IAnalyseRepository, AdoAnalyseRepository>(serviceProvider =>
            {
                return new AdoAnalyseRepository(
                    serviceProvider.GetRequiredService<DbProviderFactory>(),
                    _connectionString,
                    serviceProvider.GetRequiredService<ILogger<AdoAnalyseRepository>>()
                );
            }).AddScoped<IBodyStatRepository, AdoBodyStatRepository>(serviceProvider =>
            {
                return new AdoBodyStatRepository(
                    serviceProvider.GetRequiredService<DbProviderFactory>(),
                    _connectionString,
                    serviceProvider.GetRequiredService<ILogger<AdoBodyStatRepository>>()
                );
            });
    }

    private static IServiceCollection AddQueries(
        this IServiceCollection services
    )
    {
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
            }).AddScoped<IBodyStatsQuery, LatestBodyStatsQuery>(serviceProvider =>
            {
                return new LatestBodyStatsQuery(
                    serviceProvider.GetRequiredService<DbProviderFactory>(),
                    _connectionString,
                    serviceProvider.GetRequiredService<ILogger<LatestBodyStatsQuery>>()
                );
            });
    }

    private static IServiceCollection AddAdoServices(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        string provider = configuration["Persistence:Provider"]!;

        DbProviderFactories.RegisterFactory(provider, MySql.Data.MySqlClient.MySqlClientFactory.Instance);

        return services.AddScoped(serviceProvider => DbProviderFactories.GetFactory(provider));
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