namespace FluxConfig.Management.Domain.Models.Auth;

public record SessionModel(
    string Id,
    long UserId,
    DateTimeOffset ExpirationDate
)
{
    public const string SessionCookieKey = "fcm-session-id";
};