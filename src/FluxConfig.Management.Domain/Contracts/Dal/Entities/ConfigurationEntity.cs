namespace FluxConfig.Management.Domain.Contracts.Dal.Entities;

public class ConfigurationEntity
{
    public long Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string StorageKey { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
}