using FluxConfig.Management.Api.Contracts.Responses.Auth;
using FluxConfig.Management.Domain.Models.Auth;

namespace FluxConfig.Management.Api.Mappers.Models;

internal static class AuthModelsMapper
{
    internal static UserLoginResponse MapModelToLoginResponse(this SetCookieModel model)
    {
        return new UserLoginResponse(
            Id: model.Id,
            Email: model.Email,
            Username: model.Username,
            Roles: model.Roles.Select(r => r.ToString().ToLower())
        );
    }

    internal static UserCheckAuthResponse MapModelToCheckAuthResponse(this SetCookieModel model)
    {
        return new UserCheckAuthResponse(
            Id: model.Id,
            Email: model.Email,
            Username: model.Username,
            Roles: model.Roles.Select(r => r.ToString().ToLower())
        );
    }
}