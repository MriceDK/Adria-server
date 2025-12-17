using Adria.Main.Modules.Persistence;
using Adria.Main.Modules.UseCases;
using Adria.Main.Modules.WebApi;
using Adria.Main.Modules.PushNotifications;


var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

PersistenceModule
    .AddPersistenceModule(builder.Services, configuration);

PushNotificationModule
    .AddPersistenceModule(builder.Services, configuration);

builder.Services
    .AddWebApiModule(configuration)
    .AddUseCases();

await builder
    .Build()
    .UsePersistenceModule()
    .UseWebApiModule()
    .RunAsync(); 
