using CryingOnion.FluidSpace;
using CryingOnion.Core.Markers;
using CryingOnion.Repositories;
using {{SolutionName}}.Application;
using CryingOnion.EventPublisher;
using Microsoft.Extensions.Hosting;
using {{SolutionName}}.Domain.Services;
using CryingOnion.ExceptionHandling;
using Microsoft.Azure.Functions.Worker;
using {{SolutionName}}.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;
using {{SolutionName}}.Infrastructure.Repositories;
using CryingOnion.EventRouter.Internal.Mediator;
using CryingOnion.Repositories.Cosmos.Providers;

namespace {{SolutionName}}.AzureFunctions;

public class Program
{
    private static async Task Main(string[] args)
    {
        var cosmosDbConnectionString = Environment.GetEnvironmentVariable("CosmosDbConnectionString");

        ArgumentNullException.ThrowIfNull(cosmosDbConnectionString);

        var host = new HostBuilder()
            .ConfigureFunctionsWebApplication(config =>
            {
                config.GottaCatchExAll();
            })
            .ConfigureServices(services =>
            {
                services.AddApplicationInsightsTelemetryWorkerService();
                services.ConfigureFunctionsApplicationInsights();

                services.AddMediatR(config =>
                {
                    config.RegisterServicesFromAssembly(Reference.Assembly);
                });

                services.AddFluidSpace<CosmosFluidSpaceMappingService>(config =>
                {
                    config.AddNonIsolatedCosmosDatabaseProvider(
                        connectionString: cosmosDbConnectionString,
                        databaseName: "{{ProjectName}}");
                });

                services.AddEventPublisher(config =>
                {
                    config.AddMediatorEventRouter(typeof(IEvent));
                });

                services.AddTransient<IEventStoreRepository, CosmosEventStoreRepository>();
                services.AddTransient<IProjectionRepository, CosmosProjectionRepository>();
                services.AddTransient<ISpecificationRepository, CosmosSpecificationRepository>();
            })
            .Build();

        await host.RunAsync();
    }
}
