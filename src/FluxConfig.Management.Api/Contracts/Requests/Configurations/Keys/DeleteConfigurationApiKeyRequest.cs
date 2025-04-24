namespace FluxConfig.Management.Api.Contracts.Requests.Configurations.Keys;

// Getting configuration id through header
public record DeleteConfigurationApiKeyRequest(
    string Id
);