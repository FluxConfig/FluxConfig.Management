namespace FluxConfig.Management.Api.Contracts.Requests.Configurations.Users;

// Getting configuration id through header
public record AddUserToConfigurationRequest(
    string UserEmail
);