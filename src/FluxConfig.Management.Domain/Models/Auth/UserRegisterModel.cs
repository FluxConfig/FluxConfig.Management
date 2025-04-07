namespace FluxConfig.Management.Domain.Models.Auth;

public record UserRegisterModel(
    string Username,
    string Email,
    string Password
);