namespace FluxConfig.Management.Api.Contracts.Responses.Auth;

public record UserCheckAuthResponse(
    long Id,
    string Email,
    string Username,
    IEnumerable<string> Roles
);