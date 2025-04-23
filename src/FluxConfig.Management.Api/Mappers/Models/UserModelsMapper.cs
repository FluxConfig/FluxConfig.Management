using FluxConfig.Management.Api.Contracts.Responses.User;
using FluxConfig.Management.Domain.Models.Configuration;
using FluxConfig.Management.Domain.Models.User;

namespace FluxConfig.Management.Api.Mappers.Models;

internal static class UserModelsMapper
{
    internal static GetUserWithConfigurationsResponse MapModelToGetWithConfigsResponse(this UserModel userModel,
        IReadOnlyList<UserConfigurationsViewModel> configModels)
    {
        return new GetUserWithConfigurationsResponse(
            UserId: userModel.Id,
            Username: userModel.Username,
            Email: userModel.Email,
            Role: userModel.Role,
            Configurations: configModels.MapModelsToResponsesAll()
        );
    }
}