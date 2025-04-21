namespace FluxConfig.Management.Api.Contracts.Requests.Configurations.General;

// Getting configuration id through header
public record ChangeConfigurationDescriptionRequest(
    string NewDescription
);