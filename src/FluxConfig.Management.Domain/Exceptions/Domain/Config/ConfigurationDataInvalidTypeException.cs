using FluxConfig.Management.Domain.Models.Enums;

namespace FluxConfig.Management.Domain.Exceptions.Domain.Config;

public class ConfigurationDataInvalidTypeException: DomainException
{
    public string Key { get; }
    public string Value { get; }
    public ConfigurationValueType Type { get; }
    
    public ConfigurationDataInvalidTypeException(string? message, string key, string value, ConfigurationValueType type) : base(message)
    {
        Key = key;
        Value = value;
        Type = type;
    }
}