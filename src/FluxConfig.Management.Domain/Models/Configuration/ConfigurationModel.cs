namespace FluxConfig.Management.Domain.Models.Configuration;

public record ConfigurationModel(
    long Id,
    string Name,
    string StorageKey,
    string Description
);