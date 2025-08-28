using Adria.Main.Modules.Persistence;
using Adria.Main.Modules.UseCases;
using Adria.Main.Modules.WebApi;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder
    .Services
        .AddPersistenceModule(configuration)
        .AddWebApiModule(configuration)
        .AddUseCases();

await builder
    .Build()
    .UsePersistenceModule()
    .UseWebApiModule()
    .RunAsync();
