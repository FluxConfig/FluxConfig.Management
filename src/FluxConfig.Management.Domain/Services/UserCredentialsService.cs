using FluxConfig.Management.Domain.Models.Auth;
using FluxConfig.Management.Domain.Services.Interfaces;

namespace FluxConfig.Management.Domain.Services;

public class UserCredentialsService: IUserCredentialsService
{
    public async Task RegisterNewUser(UserRegisterModel model, CancellationToken cancellationToken)
    {
        await Task.Delay(TimeSpan.FromMilliseconds(1), cancellationToken);
        throw new NotImplementedException();
    }

    public async Task<SetCookieModel> LoginUser(UserLoginModel model, CancellationToken cancellationToken)
    {
        await Task.Delay(TimeSpan.FromMilliseconds(1), cancellationToken);
        throw new NotImplementedException();
    }

    public async Task LogoutUser(string sessionId, CancellationToken cancellationToken)
    {
        await Task.Delay(TimeSpan.FromMilliseconds(1), cancellationToken);
        throw new NotImplementedException();
    }

    public async Task<SetCookieModel> UserCheckAuth(string? sessionId, CancellationToken cancellationToken)
    {
        await Task.Delay(TimeSpan.FromMilliseconds(1), cancellationToken);
        throw new NotImplementedException();
    }
}