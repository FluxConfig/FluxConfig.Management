namespace FluxConfig.Management.Api.Contracts.Requests.Configurations.Tags;

// Getting configuration id through header
public record DeleteConfigurationTagRequest(
    long TagId
);