using System.Data.Common;
using Adria.Application.Contracts;
using Adria.Application.Contracts.Data;
using Adria.Application.FoodComposition;
using Adria.Application.Scanner;
using Adria.Application.Food;
using Adria.Domain.BodyStats;
using Adria.Domain.Food;
using Adria.Domain.PushNotifications;
using Adria.Domain.BodyStats;
using Adria.Domain.Order;
using Adria.Domain.Scanner;
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
            .AddQueries()
            .AddUseCases();
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
            .AddScoped<ISubscriptionRepository, AdoSubscriptionRepository>(sp =>
            {
                return new AdoSubscriptionRepository(
                    sp.GetRequiredService<DbProviderFactory>(),
                    _connectionString,
                    sp.GetRequiredService<ILogger<AdoSubscriptionRepository>>()
                );
            })
            .AddScoped<IUserRepository, AdoUserRepository>(sp =>
            {
                var factory = sp.GetRequiredService<DbProviderFactory>();
                return new AdoUserRepository(
                    factory,
                    _connectionString,
                    sp.GetRequiredService<ILogger<AdoUserRepository>>(),
                    new AdoSubscriptionRepository(
                        factory,
                        _connectionString,
                        sp.GetRequiredService<ILogger<AdoSubscriptionRepository>>()
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
            .AddScoped<IFoodComposition, AdoFoodCompositionRepository>(sp =>
            {
                return new AdoFoodCompositionRepository(
                    sp.GetRequiredService<DbProviderFactory>(),
                    _connectionString,
                    sp.GetRequiredService<ILogger<AdoFoodCompositionRepository>>(),
                    sp.GetRequiredService<IFood>(),
                    sp.GetRequiredService<INutrient>()
                );
            })
            .AddScoped<IAnalyseRepository, AdoAnalyseRepository>(sp =>
            {
                return new AdoAnalyseRepository(
                    sp.GetRequiredService<DbProviderFactory>(),
                    _connectionString,
                    sp.GetRequiredService<ILogger<AdoAnalyseRepository>>()
                );
            })
            .AddScoped<IScan, AdoScanFood>(sp =>
            {
                return new AdoScanFood(
                    sp.GetRequiredService<DbProviderFactory>(),
                    _connectionString,
                    sp.GetRequiredService<ILogger<AdoScanFood>>()
                );
            }).AddScoped<IBodyStatRepository, AdoBodyStatRepository>(serviceProvider =>
            {
                return new AdoBodyStatRepository(
                    serviceProvider.GetRequiredService<DbProviderFactory>(),
                    _connectionString,
                    serviceProvider.GetRequiredService<ILogger<AdoBodyStatRepository>>()
                );
            }).AddScoped<ISupplementRepository, AdoSupplementRepository>(serviceProvider =>
            {
                return new AdoSupplementRepository(
                    serviceProvider.GetRequiredService<DbProviderFactory>(),
                    _connectionString,
                    serviceProvider.GetRequiredService<ILogger<AdoSupplementRepository>>()
                );
            });
    }

    private static IServiceCollection AddQueries(
        this IServiceCollection services
    )
    {
        return services
            .AddScoped<IAllSubscriptionsQuery, AllSubscriptionsQuery>(sp =>
            {
                return new AllSubscriptionsQuery(
                    sp.GetRequiredService<DbProviderFactory>(),
                    _connectionString,
                    sp.GetRequiredService<ILogger<AllSubscriptionsQuery>>()
                );
            })
            .AddScoped<ISubscriptionByIdQuery, SubscriptionByIdQuery>(sp =>
            {
                return new SubscriptionByIdQuery(
                    sp.GetRequiredService<DbProviderFactory>(),
                    _connectionString,
                    sp.GetRequiredService<ILogger<SubscriptionByIdQuery>>()
                );
            })
            .AddScoped<IUserByIdQuery, UserByIdQuery>(sp =>
            {
                return new UserByIdQuery(
                    sp.GetRequiredService<DbProviderFactory>(),
                    _connectionString,
                    sp.GetRequiredService<ILogger<UserByIdQuery>>()
                );
            })
            .AddScoped<IAllUsersQuery, AllUsersQuery>(sp =>
            {
                return new AllUsersQuery(
                    sp.GetRequiredService<DbProviderFactory>(),
                    _connectionString,
                    sp.GetRequiredService<ILogger<AllUsersQuery>>()
                );
            })
            .AddScoped<IBodyStatsQuery, LatestBodyStatsQuery>(sp =>
            {
                return new LatestBodyStatsQuery(
                    sp.GetRequiredService<DbProviderFactory>(),
                    _connectionString,
                    sp.GetRequiredService<ILogger<LatestBodyStatsQuery>>()
                );
            }).AddScoped<IPushSubscriptionRepository, AdoPushSubscriptionRepository>(serviceProvider =>
            {
                return new AdoPushSubscriptionRepository(
                    serviceProvider.GetRequiredService<DbProviderFactory>(),
                    _connectionString,
                    serviceProvider.GetRequiredService<ILogger<AdoPushSubscriptionRepository>>()
                );
            }).AddScoped<ISupplementRepository, AdoSupplementRepository>(sp =>
            {
                return new AdoSupplementRepository(
                    sp.GetRequiredService<DbProviderFactory>(),
                    _connectionString,
                    sp.GetRequiredService<ILogger<AdoSupplementRepository>>()
                );
            }).AddScoped<IAllSupplementsQuery, AllSupplementsQuery>()
            .AddScoped<ISupplementsByIdQuery, SupplementsByIdQuery>();
    }

    private static IServiceCollection AddUseCases(this IServiceCollection services)
    {
        return services
            .AddScoped<GetRandomFood>()
            .AddScoped<IUseCase<GetFoodNutrientsInput, Task<IReadOnlyCollection<NutrientInfo>>>, GetFoodNutrients>()
            .AddScoped<ScanFood>()
            .AddScoped<GetScanHistory>();
    }

    private static IServiceCollection AddAdoServices(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        string provider = configuration["Persistence:Provider"]!;
        DbProviderFactories.RegisterFactory(provider, MySql.Data.MySqlClient.MySqlClientFactory.Instance);
        return services.AddScoped(sp => DbProviderFactories.GetFactory(provider));
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
