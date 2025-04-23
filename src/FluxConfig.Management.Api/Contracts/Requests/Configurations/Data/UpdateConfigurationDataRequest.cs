using FluxConfig.Management.Domain.Models.Enums;

namespace FluxConfig.Management.Api.Contracts.Requests.Configurations.Data;

// Getting configuration id through header
public record UpdateConfigurationDataRequest(
    long TagId,
    ConfigurationDataType DataType,
    IEnumerable<ConfigurationKeyValueRequest> Data
);

public record ConfigurationKeyValueRequest(
    string Key,
    string Value,
    ConfigurationValueType Type
);