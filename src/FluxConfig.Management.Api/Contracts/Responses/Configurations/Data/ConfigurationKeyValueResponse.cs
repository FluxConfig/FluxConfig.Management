using FluxConfig.Management.Domain.Models.Enums;

namespace FluxConfig.Management.Api.Contracts.Responses.Configurations.Data;

public record ConfigurationKeyValueResponse(
    long Id,
    string Key,
    string Value,
    ConfigurationValueType Type
);