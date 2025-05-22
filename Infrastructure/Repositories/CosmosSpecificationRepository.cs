using Microsoft.Azure.Cosmos;
using CryingOnion.FluidSpace;
using CryingOnion.Repositories.Cosmos;
using CryingOnion.Repositories.Cosmos.Providers;

namespace {{SolutionName}}.Infrastructure.Repositories;

public class CosmosSpecificationRepository(NonIsolatedCosmosDatabaseProvider provider, FluidSpaceMappingService mappingService) : SpecificationRepository
{
    protected override Container GetContainer<TSource>(string? isolationKey)
    {
        var containerId = mappingService.GetEntityMap<TSource>()?.Container ??
            throw new InvalidOperationException($"Container map not found for entity {typeof(TSource).Name}");

        return provider.GetContainer(containerId) ??
            throw new InvalidOperationException($"Container {containerId} not found");
    }
}
