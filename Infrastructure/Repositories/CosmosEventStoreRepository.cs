using Newtonsoft.Json;
using CryingOnion.FluidSpace;
using Microsoft.Azure.Cosmos;
using CryingOnion.Core.Markers;
using CryingOnion.Repositories.Cosmos;
using CryingOnion.EventPublisher.Abstractions;
using CryingOnion.Repositories.Cosmos.Providers;
using CryingOnion.Repositories.DataAccessObjects;

namespace {{SolutionName}}.Infrastructure.Repositories;

public sealed record CosmosEventStoreStream : IEventStoreStream
{
    private readonly List<IAggregateEvent> _events = [];

    [JsonProperty("id")]
    public string? Id { get; init; }

    [JsonProperty("type")]
    public string? Type { get; init; }

    [JsonProperty("_etag")]
    public string? Version { get; init; }

    [JsonProperty("events")]
    public IList<IAggregateEvent>? Events => _events;
}

public sealed class CosmosEventStoreRepository(NonIsolatedCosmosDatabaseProvider provider, FluidSpaceMappingService mappingService, IEventPublisher eventPublisher) : EventStoreRepository<CosmosEventStoreStream>(eventPublisher)
{
    private readonly NonIsolatedCosmosDatabaseProvider _provider = provider;
    private readonly FluidSpaceMappingService _mappingService = mappingService;

    protected override CosmosEventStoreStream CreateStream(string id, string type, string? version) => new()
    {
        Id = id,
        Type = type,
        Version = version
    };

    protected override Container GetContainer(string? isolationKey)
    {
        var containerId = _mappingService.GetEntityMap<CosmosEventStoreStream>()?.Container ??
            throw new InvalidOperationException($"Container map not found for entity {nameof(CosmosEventStoreStream)}");

        return _provider.GetContainer(containerId) ??
            throw new InvalidOperationException($"Container {containerId} not found");
    }
}
