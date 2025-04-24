using FluxConfig.Management.Domain.Models.User;

namespace FluxConfig.Management.Domain.Models.Auth;

public record SetCookieModel(
    UserModel User,
    SessionModel Session
);