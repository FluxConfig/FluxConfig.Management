namespace FluxConfig.Management.Api.Contracts.Requests.Configurations.General;

public record CreateConfigurationRequest(
    string Name,
    string Description
);