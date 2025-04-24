using FluxConfig.Management.Api.Contracts.Responses.Auth;
using FluxConfig.Management.Api.Contracts.Responses.User;
using FluxConfig.Management.Domain.Models.Auth;
using FluxConfig.Management.Domain.Models.User;

namespace FluxConfig.Management.Api.Mappers.Models;

internal static class AuthModelsMapper
{
    internal static UserLoginResponse MapModelToResponse(this SetCookieModel model)
    {
        return new UserLoginResponse(
            Id: model.User.Id,
            Email: model.User.Email,
            Username: model.User.Username,
            Role: model.User.Role
        );
    }

    internal static UserCheckAuthResponse MapModelToAuthResponse(this UserModel model)
    {
        return new UserCheckAuthResponse(
            Id: model.Id,
            Email: model.Email,
            Username: model.Username,
            Role: model.Role
        );
    }

    private static GetSystemUserResponse MapModelToGetSysResponse(this UserModel model)
    {
        return new GetSystemUserResponse(
            Id: model.Id,
            Email: model.Email,
            Username: model.Username,
            Role: model.Role
        );
    }

    internal static IEnumerable<GetSystemUserResponse> MapModelsToSysResponses(this IReadOnlyList<UserModel> models)
    {
        return models.Select(m => m.MapModelToGetSysResponse());
    }
}