using FluxConfig.Management.Api.Contracts.Responses.Configurations.General;
using FluxConfig.Management.Api.Contracts.Responses.Configurations.Keys;
using FluxConfig.Management.Api.Contracts.Responses.Configurations.Tags;
using FluxConfig.Management.Api.Contracts.Responses.Configurations.Users;
using FluxConfig.Management.Domain.Models.Configuration;

namespace FluxConfig.Management.Api.Mappers.Models;

internal static class ConfigModelsMapper
{
    private static GetConfigurationApiKeyResponse MapModelToResponse(this ConfigurationKeyModel model)
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

    private static GetConfigurationUserResponse MapModelToResponse(this ConfigurationUsersViewModel model)
    {
        return new GetConfigurationUserResponse(
            Id: model.UserId,
            Username: model.Username,
            Email: model.Email,
            Role: model.Role
        );
    }

    internal static IEnumerable<GetConfigurationUserResponse> MapModelsToResponses(
        this IReadOnlyList<ConfigurationUsersViewModel> models)
    {
        return models.Select(m => m.MapModelToResponse());
    }

    private static GetUserConfigurationMetaResponse MapModelToResponseAll(this UserConfigurationsViewModel model)
    {
        return new GetUserConfigurationMetaResponse(
            Id: model.ConfigurationId,
            Name: model.ConfigurationName,
            UserRole: model.Role
        );
    }

    internal static IEnumerable<GetUserConfigurationMetaResponse> MapModelsToResponsesAll(
        this IReadOnlyList<UserConfigurationsViewModel> models)
    {
        return models.Select(e => e.MapModelToResponseAll());
    }

    internal static GetConfigurationMetaResponse MapModelToMetaResponse(this UserConfigurationsViewModel model)
    {
        return new GetConfigurationMetaResponse(
            Id: model.ConfigurationId,
            Name: model.ConfigurationName,
            Description: model.ConfigurationDescription,
            UserRole: model.Role
        );
    }

    private static GetConfigurationTagResponse MapModelToResponse(this ConfigurationTagModel model)
    {
        return new GetConfigurationTagResponse(
            Id: model.Id,
            Tag: model.Tag,
            RequiredRole: model.RequiredRole
        );
    }

    internal static IEnumerable<GetConfigurationTagResponse> MapModelsToResponses(
        this IReadOnlyList<ConfigurationTagModel> models)
    {
        return models.Select(m => m.MapModelToResponse());
    }

    internal static GetConfigurationTagMetaResponse MapModelToMetaResponse(this ConfigurationTagsViewModel model)
    {
        return new GetConfigurationTagMetaResponse(
            Id: model.TagId,
            ConfigurationId: model.ConfigurationId,
            Tag: model.TagName,
            Description: model.TagDescription,
            RequiredRole: model.RequiredRole,
            ConfigurationName: model.Name
        );
    }
}