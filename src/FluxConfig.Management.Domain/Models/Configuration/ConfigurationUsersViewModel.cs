using FluxConfig.Management.Domain.Models.Enums;

namespace FluxConfig.Management.Domain.Models.Configuration;

public record ConfigurationUsersViewModel(
    long UserId,
    long ConfigurationId,
    UserConfigRole Role,
    string Username,
    string Email
);