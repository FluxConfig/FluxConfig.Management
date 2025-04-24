using FluxConfig.Management.Domain.Models.Enums;

namespace FluxConfig.Management.Domain.Models.Configuration;

public class ConfigurationKeyValueType
{
    public string Key { get; init; } = string.Empty;
    public string Value { get; init; } = string.Empty;
    public ConfigurationValueType Type { get; init; }
}