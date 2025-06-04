using CryingOnion.FluidSpace;
using {{SolutionName}}.Infrastructure.Repositories;

namespace {{SolutionName}}.Infrastructure.Services;

public class CosmosFluidSpaceMappingService : FluidSpaceMappingService
{
    protected override void MapSpace(FluidSpaceMapper mapper)
    {
        mapper.MapType<CosmosEventStoreStream>(x => x.ToContainer("event-store")
            .WithId(x => x.Id!)
            .WithPartitionKey(x => x.Type!)
            .Map());
    }
}
