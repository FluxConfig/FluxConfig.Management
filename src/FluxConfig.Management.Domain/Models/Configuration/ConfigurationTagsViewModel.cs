using FluxConfig.Management.Domain.Models.Enums;

namespace FluxConfig.Management.Domain.Models.Configuration;

public record ConfigurationTagsViewModel(
    long ConfigurationId,
    string Name,
    string StorageKey,
    string ConfigurationDescription,
    long TagId,
    string TagName,
    UserConfigRole RequiredRole,
    string TagDescription
);