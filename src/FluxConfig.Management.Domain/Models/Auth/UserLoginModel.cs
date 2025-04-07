namespace FluxConfig.Management.Domain.Models.Auth;

public record UserLoginModel(
    string Email,
    string Password,
    bool RememberUser
);