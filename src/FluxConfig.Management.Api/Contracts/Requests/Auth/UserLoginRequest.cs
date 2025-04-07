namespace FluxConfig.Management.Api.Contracts.Requests.Auth;

public record UserLoginRequest(
    string? Email,
    string? Password,
    bool? RememberUser
);