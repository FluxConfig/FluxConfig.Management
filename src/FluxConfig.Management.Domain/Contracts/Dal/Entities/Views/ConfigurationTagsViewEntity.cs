using FluxConfig.Management.Domain.Models.Enums;

namespace FluxConfig.Management.Domain.Contracts.Dal.Entities.Views;

public class ConfigurationTagsViewEntity
{
    public long ConfigurationId { get; init; }
    public string Name { get; init; } = string.Empty;
    public string StorageKey { get; init; } = string.Empty;
    public string ConfigurationDescription { get; init; } = string.Empty;
    public long TagId { get; init; }
    public string TagName { get; init; } = string.Empty;
    public UserConfigRole RequiredRole { get; init; }
    public string TagDescription { get; init; } = string.Empty;
}