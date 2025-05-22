using Microsoft.Azure.Cosmos;
using CryingOnion.FluidSpace;
using CryingOnion.Repositories.Cosmos;
using CryingOnion.Repositories.Cosmos.Providers;

namespace {{SolutionName}}.Infrastructure.Repositories;

public class CosmosProjectionRepository(NonIsolatedCosmosDatabaseProvider provider, FluidSpaceMappingService mappingService) : ProjectionRepository
{
    protected override Container GetContainer<TProjection>(string? isolationKey)
    {
        var containerId = mappingService.GetEntityMap<TProjection>()?.Container ??
            throw new InvalidOperationException($"Container map not found for entity {typeof(TProjection).Name}");

        return provider.GetContainer(containerId) ??
            throw new InvalidOperationException($"Container {containerId} not found");
    }
}
