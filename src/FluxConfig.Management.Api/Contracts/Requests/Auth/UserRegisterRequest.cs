namespace FluxConfig.Management.Api.Contracts.Requests.Auth;

public record UserRegisterRequest(
    string? Username,
    string? Email,
    string Password
);