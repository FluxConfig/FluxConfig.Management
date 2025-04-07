using FluxConfig.Management.Domain.Models.Auth;

namespace FluxConfig.Management.Domain.Services.Interfaces;

public interface IUserCredentialsService
{
    public Task RegisterNewUser(UserRegisterModel model, CancellationToken cancellationToken);

    public Task<SetCookieModel> LoginUser(UserLoginModel model, CancellationToken cancellationToken);

    public Task LogoutUser(string sessionId, CancellationToken cancellationToken);

    public Task<SetCookieModel> UserCheckAuth(string? sessionId, CancellationToken cancellationToken);
}