namespace FluxConfig.Management.Api.Contracts.Requests.Configurations.Users;

// Getting configuration id through header
public record DeleteUserFromConfigurationRequest(
    long UserId
);