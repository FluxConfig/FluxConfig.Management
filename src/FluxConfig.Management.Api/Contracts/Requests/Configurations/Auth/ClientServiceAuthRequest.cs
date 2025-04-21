namespace FluxConfig.Management.Api.Contracts.Requests.Configurations.Auth;

public record ClientServiceAuthRequest(
    string ApiKey,
    string Tag
);