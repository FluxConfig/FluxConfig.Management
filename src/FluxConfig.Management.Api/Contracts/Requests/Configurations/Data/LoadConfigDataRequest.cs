using FluxConfig.Management.Domain.Models.Enums;

namespace FluxConfig.Management.Api.Contracts.Requests.Configurations.Data;

// Getting configuration id through header
public record LoadConfigDataRequest(
    long TagId,
    ConfigurationDataType DataType
);