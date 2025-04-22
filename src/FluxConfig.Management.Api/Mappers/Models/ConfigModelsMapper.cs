using FluxConfig.Management.Api.Contracts.Responses.Configurations.Keys;
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
    
    internal static IEnumerable<GetConfigurationApiKeyResponse> MapModelsToResponses(this IReadOnlyList<ConfigurationKeyModel> models)
    {
        return models.Select(m => m.MapModelToResponse());
    }
}