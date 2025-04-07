using FluxConfig.Management.Domain.Models.Enums;

namespace FluxConfig.Management.Domain.Models.Auth;

public record SetCookieModel(
    long Id,
    string Username,
    string Email,
    string SessionId,
    DateTimeOffset ExpirationDate,
    IReadOnlyList<UserGlobalRole> Roles
)
{
    public const string CookieKey = "fcm-session-id";
};