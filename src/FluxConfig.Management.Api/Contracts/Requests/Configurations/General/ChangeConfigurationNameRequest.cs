namespace FluxConfig.Management.Api.Contracts.Requests.Configurations.General;

// Getting configuration id through header
public record ChangeConfigurationNameRequest(
    string NewName
);