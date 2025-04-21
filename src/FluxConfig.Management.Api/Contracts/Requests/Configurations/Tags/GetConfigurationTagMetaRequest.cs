using System.Text.Json.Serialization;

namespace FluxConfig.Management.Api.Contracts.Requests.Configurations.Tags;

// Getting configuration id through header
public record GetConfigurationTagMetaRequest(
    long TagId
);