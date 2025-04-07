namespace FluxConfig.Management.Api.Contracts.Responses.Auth;

public record UserLoginResponse(
    long Id,
    string Email,
    string Username,
    IEnumerable<string> Roles
);