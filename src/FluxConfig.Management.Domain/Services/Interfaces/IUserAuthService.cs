using FluxConfig.Management.Domain.Models.Auth;
using FluxConfig.Management.Domain.Models.User;

namespace FluxConfig.Management.Domain.Services.Interfaces;

public interface IUserAuthService
{
    public Task RegisterNewUser(UserRegisterModel model, CancellationToken cancellationToken);

    public Task<SetCookieModel> LoginUser(UserLoginModel model, CancellationToken cancellationToken);

    public Task LogoutUser(string sessionId, CancellationToken cancellationToken);

    public Task<UserModel> UserCheckAuth(string? sessionId, CancellationToken cancellationToken);
}