using FluxConfig.Management.Api.Contracts.Responses.Configurations.Keys;
using FluxConfig.Management.Api.Contracts.Responses.Configurations.Users;
using FluxConfig.Management.Domain.Models.Configuration;

namespace FluxConfig.Management.Api.Mappers.Models;

internal static class ConfigModelsMapper
{
    internal static GetConfigurationApiKeyResponse MapModelToResponse(this ConfigurationKeyModel model)
    {
        return new GetConfigurationApiKeyResponse(
            Id: model.Id,
            RolePermission: model.RolePermission,
            ExpirationDate: model.ExpirationDate.Date
        );
    }

    internal static IEnumerable<GetConfigurationApiKeyResponse> MapModelsToResponses(
        this IReadOnlyList<ConfigurationKeyModel> models)
    {
        return models.Select(m => m.MapModelToResponse());
    }

    internal static GetConfigurationUserResponse MapModelToResponse(this ConfigurationUsersViewModel model)
    {
        return new GetConfigurationUserResponse(
            Id: model.UserId,
            Username: model.Username,
            Email: model.Email,
            Role: model.Role
        );
    }
    
    internal static IEnumerable<GetConfigurationUserResponse> MapModelsToResponses(this IReadOnlyList<ConfigurationUsersViewModel> models)
    {
        return models.Select(m => m.MapModelToResponse());
    }
}